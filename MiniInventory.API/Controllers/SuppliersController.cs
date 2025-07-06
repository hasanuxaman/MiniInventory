using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _repo;

        public SuppliersController(ISupplierRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("GetAllSupplier")]
        public async Task<IActionResult> GetAllSupplier() => Ok(await _repo.GetAllAsync());

        [HttpGet]
        [Route("GetSupplierById")]

        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _repo.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        [Route("AddSupplier")]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            await _repo.AddAsync(supplier);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateSupplierById")]

        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {
            await _repo.UpdateAsync(supplier);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteSupplierById")]

        public async Task<IActionResult> DeleteSupplierById(int id)
        {
            await _repo.DeleteAsync(id);
            return Ok();
        }
    }
}
