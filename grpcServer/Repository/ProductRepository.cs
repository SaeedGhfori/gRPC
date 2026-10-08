using grpcServer.Models;
using System;

namespace grpcServer.Repository
{
    public interface IProductRepository
    {
        int Add(string name, int price, CancellationToken cancellationToken = default);
        List<Product> GetProducts(CancellationToken cancellationToken = default);
        Product Update(Product product, CancellationToken cancellationToken = default);
        void Remove(int id, CancellationToken cancellationToken = default);
        Product GetById(int id, CancellationToken cancellationToken = default);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public int Add(string name, int price, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int id;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                id = Random.Shared.Next(1, int.MaxValue);
            } while (_products.Any(p => p.Id == id));

            _products.Add(new Product
            {
                Id = id,
                Name = name,
                Price = price,
            });

            return id;
        }
        public List<Product> GetProducts(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _products;
        }
        public Product Update(Product product, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with Id {product.Id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            return existingProduct;
        }
        public void Remove(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
        public Product GetById(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var product = _products.FirstOrDefault(i => i.Id == id);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            return product;
        }
    }
}
