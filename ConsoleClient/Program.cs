using Grpc.Net.Client;
using grpcServer.Protos;

Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("https://localhost:7164/");

var productClient = new ProductService.ProductServiceClient(channel);
var response = productClient.AddNewProduct(new RequestAddProductDto
{
    Name = "acer",
    Brand = "acer",
    Price = 167000,
});

Console.WriteLine(response.IsSuccess);
Console.ReadLine();