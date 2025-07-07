using MiniInventory.API.DTOs;

namespace MiniInventory.API.Interfaces
{
    public interface IStockReportRepository
    {
        Task<IEnumerable<StockReportDto>> GetStockReportAsync();
    }
}
