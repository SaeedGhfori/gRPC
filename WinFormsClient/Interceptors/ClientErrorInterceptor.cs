using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

namespace WinFormsClient.Interceptors
{

    /// <summary>
/// اینترسپتور سمت کلاینت برای WinForms: خطای کد ماشینی (x-error-code) را از تریلر می‌گیرد
/// و در LastErrorCode ذخیره می‌کند تا WinForms بدون parse کردن متن پیام تصمیم بگیرد.
/// همه‌ی انواع کال‌های gRPC (Unary / Client Stream / Server Stream / Duplex) پوشش داده شده.
/// </summary>
public class ClientErrorInterceptor : Interceptor
{
    public string? LastErrorCode { get; private set; }

    public override AsyncUnaryCall<TRequest, TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncUnaryCall<TRequest, TResponse>(
            HandleAsync(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(context);
        return new AsyncClientStreamingCall<TRequest, TResponse>(
            call.RequestStream,
            HandleAsync(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    public override AsyncServerStreamingCall<TRequest, TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncServerStreamingCall<TRequest, TResponse>(
            new ErrorCapturingStreamReader<TResponse>(call.ResponseStream, this),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(context);
        return new AsyncDuplexStreamingCall<TRequest, TResponse>(
            call.RequestStream,
            new ErrorCapturingStreamReader<TResponse>(call.ResponseStream, this),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    internal void Capture(RpcException ex) =>
        LastErrorCode = ex.Trailers.GetValue("x-error-code") ?? "Unknown";

    private async Task<TResponse> HandleAsync<TResponse>(Task<TResponse> responseTask)
    {
        try
        {
            return await responseTask;
        }
        catch (RpcException ex)
        {
            Capture(ex);
            throw;
        }
    }

    private sealed class ErrorCapturingStreamReader<T> : IAsyncStreamReader<T>
    {
        private readonly IAsyncStreamReader<T> _inner;
        private readonly ClientErrorInterceptor _interceptor;

        public ErrorCapturingStreamReader(IAsyncStreamReader<T> inner, ClientErrorInterceptor interceptor)
        {
            _inner = inner;
            _interceptor = interceptor;
        }

        public T Current => _inner.Current;

        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _inner.MoveNextAsync(cancellationToken);
            }
            catch (RpcException ex)
            {
                _interceptor.Capture(ex);
                throw;
            }
        }
    }
}