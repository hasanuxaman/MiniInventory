using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _repo;

        public OrderDetailsController(IOrderDetailRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var details = await _repo.GetByOrderIdAsync(orderId);
            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDetail detail)
        {
            await _repo.AddAsync(detail);
            return Ok();
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> Delete(int orderId, int productId)
        {
            await _repo.DeleteAsync(orderId, productId);
            return Ok();
        }
    }

}
