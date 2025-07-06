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
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var transactions = await _repo.GetByProductIdAsync(productId);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Add(InventoryTransaction transaction)
        {
            await _repo.AddAsync(transaction);
            return Ok();
        }
    }

}
