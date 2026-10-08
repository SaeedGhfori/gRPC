using Grpc.Core;

namespace grpcServer.GRPC;

/// <summary>
/// اکسپشن دامنه‌ای برای برگرداندن یک خطای مشخص gRPC از داخل سرویس.
/// مثال: throw new GrpcServiceException(StatusCode.AlreadyExists, "این محصول قبلاً ثبت شده است.");
/// </summary>
public sealed class GrpcServiceException : Exception
{
    public StatusCode StatusCode { get; }

    public GrpcServiceException(StatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
