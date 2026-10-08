using grpcServer.Models;

namespace grpcServer.Repository
{
    public interface IProductRepository
    {
        int Add(string name, int price);
        List<Product> GetProducts();
        Product Update(Product product);
        void Remove(int id);
        Product GetById(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public int Add(string name, int price)
        {
            int id;
            do
            {
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
        public List<Product> GetProducts()
        {
            return _products;
        }
        public Product Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with Id {product.Id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            return existingProduct;
        }
        public void Remove(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(i => i.Id == id);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            return product;
        }
    }
}
