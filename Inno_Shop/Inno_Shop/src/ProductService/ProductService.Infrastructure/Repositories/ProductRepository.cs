using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private readonly object _lock = new();

        public Task<Product?> GetByIdAsync(Guid id)
        {
            lock (_lock)
            {
                return Task.FromResult(_products.FirstOrDefault(p => p.Id == id && p.IsAvailable));
            }
        }

        public Task<List<Product>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_products.Where(p => p.IsAvailable).ToList());
            }
        }

        public Task<List<Product>> GetByUserIdAsync(Guid userId)
        {
            lock (_lock)
            {
                return Task.FromResult(_products.Where(p => p.UserId == userId && p.IsAvailable).ToList());
            }
        }

        public Task AddAsync(Product product)
        {
            lock (_lock)
            {
                _products.Add(product);
                return Task.CompletedTask;
            }
        }

        public Task UpdateAsync(Product product)
        {
            lock (_lock)
            {
                var existing = _products.FirstOrDefault(p => p.Id == product.Id);
                if (existing != null)
                {
                    _products.Remove(existing);
                    _products.Add(product);
                }
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync(Guid id)
        {
            lock (_lock)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    product.IsAvailable = false;
                    product.UpdatedAt = DateTime.UtcNow;
                }
                return Task.CompletedTask;
            }
        }
    }
}