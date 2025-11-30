using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                UserId = createProductDto.UserId,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                UserId = product.UserId,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<List<ProductResponseDto>> GetProductsByUserAsync(Guid userId)
        {
            var products = await _productRepository.GetByUserIdAsync(userId);
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto, Guid userId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.UserId != userId) return false;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id, Guid userId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.UserId != userId) return false;

            await _productRepository.DeleteAsync(id);
            return true;
        }

        private static ProductResponseDto MapToDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsAvailable = product.IsAvailable,
                UserId = product.UserId,
                CreatedAt = product.CreatedAt
            };
        }
    }
}