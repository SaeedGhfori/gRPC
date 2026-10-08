using System.Security.Authentication;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace grpcServer.GRPC;

/// <summary>
/// اینترسپتور سراسری سرور: هر اکسپشنی که از سرویس بالا بیاید را به یک کد
/// استاندارد gRPC تبدیل می‌کند تا فرانت (کنسول/وین‌فرم) همیشه StatusCode
/// معنادار بگیرد، نه خطای مبهم Unknown.
///
/// پوشش هر ۱۷ کد gRPC:
/// OK (0)                 خطا نیست.
/// CANCELLED (1)           لغو توسط کلاینت → دست‌نخورده عبور می‌کند.
/// UNKNOWN (2)             فقط اگر سرویس خودش RpcException(Unknown) بدهد.
/// INVALID_ARGUMENT (3)    ArgumentException، FormatException.
/// DEADLINE_EXCEEDED (4)   TimeoutException، یا لغو به‌علت پایان مهلت.
/// NOT_FOUND (5)           KeyNotFoundException، File/DirectoryNotFound.
/// ALREADY_EXISTS (6)      با GrpcServiceException از داخل سرویس.
/// PERMISSION_DENIED (7)   UnauthorizedAccessException.
/// RESOURCE_EXHAUSTED (8)  با GrpcServiceException (سهمیه/محدودیت).
/// FAILED_PRECONDITION (9) InvalidOperationException.
/// ABORTED (10)            با GrpcServiceException (تعارض همزمانی).
/// OUT_OF_RANGE (11)       ArgumentOutOfRangeException، OverflowException.
/// UNIMPLEMENTED (12)      NotImplementedException، NotSupportedException.
/// INTERNAL (13)           هر خطای پیش‌بینی‌نشده (بدون لو رفتن جزئیات).
/// UNAVAILABLE (14)        IOException (خطای موقت زیرساخت).
/// DATA_LOSS (15)          با GrpcServiceException (خرابی داده).
/// UNAUTHENTICATED (16)    AuthenticationException.
///
/// تریلرها: علاوه بر Status، کلید ماشینی x-error-code (مثل "NotFound")
/// در تریلر پاسخ گذاشته می‌شود تا فرانت بدون parse کردن متن پیام تصمیم بگیرد.
/// </summary>
public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex) when (ShouldHandle(ex, context))
        {
            throw Handle(ex, context);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception ex) when (ShouldHandle(ex, context))
        {
            throw Handle(ex, context);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex) when (ShouldHandle(ex, context))
        {
            throw Handle(ex, context);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex) when (ShouldHandle(ex, context))
        {
            throw Handle(ex, context);
        }
    }

    /// <summary>
    /// خطای از قبل نگاشت‌شده (RpcException) و لغو واقعی توسط کلاینت
    /// دست‌نخورده عبور می‌کنند؛ فقط لغوِ ناشی از پایان مهلت به DeadlineExceeded تبدیل می‌شود.
    /// </summary>
    private static bool ShouldHandle(Exception ex, ServerCallContext context)
    {
        if (ex is RpcException)
            return false;

        if (ex is OperationCanceledException)
            return DateTime.UtcNow >= context.Deadline;

        return true;
    }

    internal static (StatusCode Code, string Message) Map(Exception ex) =>
        ex switch
        {
            GrpcServiceException serviceEx => (serviceEx.StatusCode, serviceEx.Message),
            OperationCanceledException => (StatusCode.DeadlineExceeded, "مهلت درخواست به پایان رسید."),
            KeyNotFoundException => (StatusCode.NotFound, ex.Message),
            FileNotFoundException => (StatusCode.NotFound, ex.Message),
            DirectoryNotFoundException => (StatusCode.NotFound, ex.Message),
            ArgumentOutOfRangeException => (StatusCode.OutOfRange, ex.Message),
            ArgumentException => (StatusCode.InvalidArgument, ex.Message),
            FormatException => (StatusCode.InvalidArgument, ex.Message),
            OverflowException => (StatusCode.OutOfRange, ex.Message),
            InvalidOperationException => (StatusCode.FailedPrecondition, ex.Message),
            NotImplementedException => (StatusCode.Unimplemented, ex.Message),
            NotSupportedException => (StatusCode.Unimplemented, ex.Message),
            TimeoutException => (StatusCode.DeadlineExceeded, ex.Message),
            AuthenticationException => (StatusCode.Unauthenticated, ex.Message),
            UnauthorizedAccessException => (StatusCode.PermissionDenied, ex.Message),
            IOException => (StatusCode.Unavailable, "سرویس موقتاً در دسترس نیست."),
            _ => (StatusCode.Internal, "خطای داخلی سرور.")
        };

    private RpcException Handle(Exception ex, ServerCallContext context)
    {
        var (code, message) = Map(ex);

        // کد ماشینی برای فرانت؛ کلید/مقدار تریلر باید ASCII باشد.
        // فرانت با ex.Trailers.GetValue("x-error-code") می‌خواند: "NotFound"، "InvalidArgument" و...
        context.ResponseTrailers.Add("x-error-code", code.ToString());

        if (code is StatusCode.Internal or StatusCode.DataLoss or StatusCode.Unknown)
            _logger.LogError(ex, "gRPC {Method} failed with {StatusCode}", context.Method, code);
        else
            _logger.LogWarning(ex, "gRPC {Method} rejected with {StatusCode}: {Message}", context.Method, code, message);

        return new RpcException(new Status(code, message));
    }
}
