using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _repo.GetAllAsync();

           
        //    if (products == null || !products.Any())
        //    {
        //        products = new List<Product>
        //{
        //    new Product { Id = 1, Name = "Demo Mouse", Quantity = 10, Price = 250 },
        //    new Product { Id = 2, Name = "Demo Keyboard", Quantity = 5, Price = 550 },
        //    new Product { Id = 2, Name = "Demo PC", Quantity = 5, Price = 10000 }
        //};
        //    }

            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _repo.GetByIdAsync(id); return p == null ? NotFound() : Ok(p);
        }
        [HttpPost] 
        public async Task<IActionResult> Post(Product product) 
        { 
            await _repo.AddAsync(product); 
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product); 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Product p)
        {
            if (id != p.Id) return BadRequest();
            await _repo.UpdateAsync(p);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
