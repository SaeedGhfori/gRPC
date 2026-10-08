using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace grpcServer.GRPC;

/// <summary>
/// Server interceptor for gRPC logging and timeout handling.
/// Logs method name, remaining deadline, cancellation state,
/// execution time, final status code, and any exceptions.
/// </summary>
public class ServerInterceptor : Interceptor
{
    private readonly ILogger<ServerInterceptor> _logger;

    public ServerInterceptor(ILogger<ServerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var methodName = context.Method;
        var deadline = context.Deadline?.UtcDateTime ?? DateTime.UtcNow.AddDays(1); // Default 1 day if no deadline
        var cancellationToken = context.CancellationToken;
        var startTime = DateTime.UtcNow;

        _logger.LogInformation(
            "gRPC {Method} called - Deadline: {Deadline:yyyy-MM-dd HH:mm:ss}, CancellationRequested: {CancellationRequested}, Client: {Client}",
            methodName, deadline, cancellationToken.IsCancellationRequested, context.Peer);

        try
        {
            var response = await continuation(request, context);
            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                "gRPC {Method} completed successfully - Duration: {Duration:mm\:ss\.fff}ms, Status: OK",
                methodName, duration);
            return response;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(
                ex,
                "gRPC {Method} failed - Duration: {Duration:mm\:ss\.fff}ms, Error: {Error}",
                methodName, duration, ex.Message);

            // Convert to RpcException for gRPC compliance
            var statusCode = ex switch
            {
                KeyNotFoundException => StatusCode.NotFound,
                ArgumentException => StatusCode.InvalidArgument,
                InvalidOperationException => StatusCode.FailedPrecondition,
                OperationCanceledException when context.Deadline <= DateTime.UtcNow => StatusCode.DeadlineExceeded,
                OperationCanceledException => StatusCode.Cancelled,
                AuthenticationException => StatusCode.Unauthenticated,
                UnauthorizedAccessException => StatusCode.PermissionDenied,
                TimeoutException => StatusCode.DeadlineExceeded,
                NotImplementedException => StatusCode.Unimplemented,
                NotSupportedException => StatusCode.Unimplemented,
                ArgumentOutOfRangeException => StatusCode.OutOfRange,
                _ => StatusCode.Internal
            };

            var rpcException = new RpcException(new Status(statusCode, ex.Message));
            _logger.LogError(
                "gRPC {Method} threw RpcException - Status: {StatusCode}, Message: {Message}",
                methodName, statusCode, ex.Message);
            throw rpcException;
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var methodName = context.Method;
        var deadline = context.Deadline?.UtcDateTime ?? DateTime.UtcNow.AddDays(1);
        var cancellationToken = context.CancellationToken;
        var startTime = DateTime.UtcNow;

        _logger.LogInformation(
            "gRPC {Method} client streaming - Deadline: {Deadline:yyyy-MM-dd HH:mm:ss}, CancellationRequested: {CancellationRequested}, Client: {Client}",
            methodName, deadline, cancellationToken.IsCancellationRequested, context.Peer);

        try
        {
            var response = await continuation(requestStream, context);
            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                "gRPC {Method} client streaming completed - Duration: {Duration:mm\:ss\.fff}ms, Status: OK",
                methodName, duration);
            return response;
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(
                ex,
                "gRPC {Method} client streaming failed - Duration: {Duration:mm\:ss\.fff}ms, Error: {Error}",
                methodName, duration, ex.Message);
            throw;
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var methodName = context.Method;
        var deadline = context.Deadline?.UtcDateTime ?? DateTime.UtcNow.AddDays(1);
        var cancellationToken = context.CancellationToken;
        var startTime = DateTime.UtcNow;

        _logger.LogInformation(
            "gRPC {Method} server streaming - Deadline: {Deadline:yyyy-MM-dd HH:mm:ss}, CancellationRequested: {CancellationRequested}, Client: {Client}",
            methodName, deadline, cancellationToken.IsCancellationRequested, context.Peer);

        try
        {
            await continuation(request, responseStream, context);
            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                "gRPC {Method} server streaming completed - Duration: {Duration:mm\:ss\.fff}ms, Status: OK",
                methodName, duration);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(
                ex,
                "gRPC {Method} server streaming failed - Duration: {Duration:mm\:ss\.fff}ms, Error: {Error}",
                methodName, duration, ex.Message);
            throw;
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        var methodName = context.Method;
        var deadline = context.Deadline?.UtcDateTime ?? DateTime.UtcNow.AddDays(1);
        var cancellationToken = context.CancellationToken;
        var startTime = DateTime.UtcNow;

        _logger.LogInformation(
            "gRPC {Method} duplex streaming - Deadline: {Deadline:yyyy-MM-dd HH:mm:ss}, CancellationRequested: {CancellationRequested}, Client: {Client}",
            methodName, deadline, cancellationToken.IsCancellationRequested, context.Peer);

        try
        {
            await continuation(requestStream, responseStream, context);
            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation(
                "gRPC {Method} duplex streaming completed - Duration: {Duration:mm\:ss\.fff}ms, Status: OK",
                methodName, duration);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            _logger.LogError(
                ex,
                "gRPC {Method} duplex streaming failed - Duration: {Duration:mm\:ss\.fff}ms, Error: {Error}",
                methodName, duration, ex.Message);
            throw;
        }
    }
}