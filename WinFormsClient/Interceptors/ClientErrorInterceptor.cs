using Grpc.Core;
using Grpc.Core.Interceptors;

namespace WinFormsClient.Interceptors;

/// <summary>
/// اینترسپتور سمت کلاینت: کد Machin خطا (x-error-code) را از تریلر پاسخ می‌گیرد
/// و LastErrorCode را برای استفاده در فرم WinForms تنظیم می‌کند.
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
        try
        {
            //catch در caught
        }
        catch (RpcException ex)
        {
            LastErrorCode = ex.Trailers.GetValue("x-error-code");
        }
        return new AsyncUnaryCall<TRequest, TResponse>(
            call.ResponseAsync,
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    // سایر انواعカルلupported (بدون ثبت کد Machin) برای سازگاری
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        => continuation(context);

    public override AsyncServerStreamingCall<TRequest, TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        => continuation(request, context);

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        => continuation(context);
}