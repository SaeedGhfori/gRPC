using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using grpcServer.Models;
using grpcServer.Protos;
using grpcServer.Repository;

namespace grpcServer.GRPC
{
    public class ProductgRPCService : ProductService.ProductServiceBase
    {
        private readonly IProductRepository _repository;

        public ProductgRPCService(IProductRepository repository)
        {
            _repository = repository;
        }

        public override async Task AddProduct(
            IAsyncStreamReader<ProductRequest> requestStream,
            IServerStreamWriter<ProductReply> responseStream,
            ServerCallContext context)
        {
            var deadline = context.Deadline;
            if (deadline.HasValue && DateTime.UtcNow >= deadline.Value)
            {
                throw new RpcException(new Status(StatusCode.DeadlineExceeded, "Deadline exceeded before processing started."));
            }

            await foreach (var request in requestStream.ReadAllAsync())
            {
                if (deadline.HasValue && DateTime.UtcNow >= deadline.Value)
                {
                    throw new RpcException(new Status(StatusCode.DeadlineExceeded, "Deadline exceeded while processing request."));
                }

                var id = _repository.Add(request.Name, request.Price, context.CancellationToken);

                var product = _repository.GetById(id, context.CancellationToken);

                await responseStream.WriteAsync(new ProductReply
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                }, cancellationToken: context.CancellationToken);
            }
        }

        public override async Task<ProductReply> UpdateProduct(
            ProductRequest request,
            ServerCallContext context)
        {
            var product = _repository.Update(new Product
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price
            });

            return await Task.FromResult(new ProductReply
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        public override async Task<ProductReply> GetProductById(
            ProductByIdRequest request,
            ServerCallContext context)
        {
            var product = _repository.GetById(request.Id);

            return await Task.FromResult(new ProductReply
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });
        }

        public override async Task<Empty> DeleteProduct(
            IAsyncStreamReader<ProductByIdRequest> requestStream,
            ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                _repository.Remove(request.Id);
            }

            return new Empty();
        }

        public override async Task GetAllProduct(
            Empty request,
            IServerStreamWriter<ProductReply> responseStream,
            ServerCallContext context)
        {
            var products = _repository.GetProducts();

            foreach (var product in products)
            {
                await responseStream.WriteAsync(new ProductReply
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                });
            }
        }

        public override Task<ResponseAllProduct> GetAll(
            RequestAllProduct request,
            ServerCallContext context)
        {
            var products = _repository.GetProducts();

            var pagedProducts = products
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var response = new ResponseAllProduct();

            response.Items.AddRange(pagedProducts.Select(p => new ProductReply
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }));

            return Task.FromResult(response);
        }
    }
}