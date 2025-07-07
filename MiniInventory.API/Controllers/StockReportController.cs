using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.API.Interfaces;

namespace MiniInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReportController : ControllerBase
    {
        private readonly IStockReportRepository _repository;

        public StockReportController(IStockReportRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetStockReport")]
        public async Task<IActionResult> GetStockReport()
        {
            var report = await _repository.GetStockReportAsync();
            return Ok(report);
        }

    }
}
