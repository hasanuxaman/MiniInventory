using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IStockRepository
    {

        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task<StockTransaction?> GetByIdAsync(int id);
        Task AddAsync(StockTransaction stock);
        Task UpdateAsync(StockTransaction stock);
        Task DeleteAsync(int id);
        Task StockInAsync(int productId, int qty);
        Task StockOutAsync(int productId, int qty);
     
        Task<int> GetCurrentStockAsync(int productId);
    }
}
