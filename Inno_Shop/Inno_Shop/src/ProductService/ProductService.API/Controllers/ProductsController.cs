using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> _products = new();

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products.Where(p => p.IsAvailable));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _products.Add(product);

            return Ok(new
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Message = "Product created successfully"
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsAvailable);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetProductsByUser(Guid userId)
        {
            var products = _products.Where(p => p.UserId == userId && p.IsAvailable);
            return Ok(products);
        }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid UserId { get; set; }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}