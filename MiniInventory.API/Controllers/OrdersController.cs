using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using MiniInventory.API.Repositories;

namespace MiniInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllOrder")]

        public async Task<IActionResult> GetAllOrder() => Ok(await _repo.GetAllOrdersAsync());

        [HttpGet]
        [Route("GetOrderById")]

        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            var orderId = await _repo.AddOrderAsync(order);
            return Ok(new { OrderId = orderId });
        }

        [HttpPut]
        [Route("UpdateOrderbyId")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order orderDto)
        {
            var success = await _repo.UpdateAsync(id, orderDto);
            if (success)
                return Ok(new { Message = "Order updated successfully." });
            else
                return BadRequest(new { Message = "Failed to update order." });
        }

        [HttpDelete]
        [Route("DeleteOrderById")]

        public async Task<IActionResult> DeleteOrderById(int id)
        {
            await _repo.DeleteAsync(id);
            return Ok();
        }
    }

}
