using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repo;

        public CustomersController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer() => Ok(await _repo.GetAllAsync());

        [HttpGet]
        [Route("GetCustomerbyId")]
        public async Task<IActionResult> GetCustomerbyId(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            return customer is null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [Route("AddCustomer")]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _repo.AddAsync(customer);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateCustomerbyId")]
        public async Task<IActionResult> UpdateCustomer(int id,Customer customer)
        {
            customer.CustomerId = id;
            await _repo.UpdateAsync(customer);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteCustomerById")]
        public async Task<IActionResult> DeleteCustomerById(int id)
        {
            await _repo.DeleteAsync(id);
            return Ok();
        }
    }

}
