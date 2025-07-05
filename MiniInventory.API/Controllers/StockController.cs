using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{
   
    [ApiController]

    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {

        private readonly IStockRepository _repo;

        public StockController(IStockRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("in")]
        public async Task<IActionResult> StockIn([FromBody] StockTransaction transaction)
        {
            if (transaction == null || transaction.ProductId <= 0 || transaction.Quantity <= 0)
                return BadRequest("Invalid stock input.");

            await _repo.StockInAsync(transaction.ProductId, transaction.Quantity);
            return Ok(new { message = "Stock In successful" });
        }

  
        [HttpPost("out")]
        public async Task<IActionResult> StockOut([FromBody] StockTransaction transaction)
        {
            if (transaction == null || transaction.ProductId <= 0 || transaction.Quantity <= 0)
                return BadRequest("Invalid stock output.");

            try
            {
                await _repo.StockOutAsync(transaction.ProductId, transaction.Quantity);
                return Ok(new { message = "Stock Out successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("current/{productId}")]
        public async Task<IActionResult> GetCurrentStock(int productId)
        {
            var stock = await _repo.GetCurrentStockAsync(productId);
            return Ok(new { productId, currentStock = stock });
        }

     
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list);
        }
    }
}
    