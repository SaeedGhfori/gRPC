using Grpc.AspNetCore.Server;
using Grpc.AspNetCore.Server.Reflection;
using grpcServer.GRPC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    // Configure message size limits
    options.MaxReceiveMessageSize = 10 * 1024 * 1024; // 10MB
    options.MaxSendMessageSize = 10 * 1024 * 1024; // 10MB

    // Configure compression
    options.EnableMessageCompression = true;
    options.CompressionProviders = new List<CompressionProvider>
    {
        new CompressionProvider("gzip", new GZipCompressionProvider(CompressionLevel.Fastest))
    };

    // Configure keepalive
    options.KeepAliveOptions = new KeepAliveOptions
    {
        AllowKeepAliveWithoutCalls = true,
        KeepAliveTime = TimeSpan.FromMinutes(2),
        KeepAliveTimeout = TimeSpan.FromSeconds(20)
    };

    // Enable deadline checking for all methods
    options.EnableDetailedExceptions = true;
});

// Register services
builder.Services.AddSingleton<ProductgRPCService>();
builder.Services.AddSingleton<ServerInterceptor>();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ServerInterceptor>();
});

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseRouting();

// Map gRPC services
app.MapGrpcService<ProductgRPCService>();
app.MapGrpcService<ProductgRPCService>(); // Duplicate to test

// Map gRPC reflection endpoint
app.MapGrpcReflectionService();

app.Run();