using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactionsController : ControllerBase
    {
        private readonly IInventoryTransactionRepository _repo;

        public InventoryTransactionsController(IInventoryTransactionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllStockTransectionl")]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet]
        [Route("GetStockTransectionlByProductId")]
        public async Task<IActionResult> GetStockTransectionlByProductId(int productId)
        {
            var transactions = await _repo.GetByProductIdAsync(productId);
            return Ok(transactions);
        }

        [HttpPost]
        [Route("AddStockTransectionl")]
        public async Task<IActionResult> AddStockTransectionl(InventoryTransaction transaction)
        {
            
            var result = await _repo.AddAsync(transaction);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return  BadRequest(result.Message);
            }
            return Ok("Stock transaction saved successfully.");
        }
    }

}
