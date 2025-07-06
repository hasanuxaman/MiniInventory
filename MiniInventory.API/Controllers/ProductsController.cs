using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using MiniInventory.API.Repositories;

namespace MiniInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo) => _repo = repo;

        [HttpGet]
        [Route("GetProductAll")]
        public async Task<IActionResult> GetProductAll()
        {
            var products = await _repo.GetAllAsync();

            return Ok(products);
        }
        [HttpGet]
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var p = await _repo.GetByIdAsync(id); return p == null ? NotFound() : Ok(p);
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product) 
        { 
            await _repo.AddAsync(product); 
            return CreatedAtAction(nameof(GetProductAll), new { id = product.Id }, product); 
        }
        [HttpPut]
        [Route("UpdateProductById")]
        public async Task<IActionResult> UpdateProductById(int Id, Product product)
        {
            var exists = await _repo.GetByIdAsync(Id);
            await _repo.UpdateAsync(product);
            return Ok(product);
        }
        [HttpDelete]
        [Route("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
