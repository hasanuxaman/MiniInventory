using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task<IEnumerable<InventoryTransactionDto>> GetAllAsync();
        Task<IEnumerable<InventoryTransaction>> GetByProductIdAsync(int productId);
        Task<(bool IsSuccess, string Message)> AddAsync(InventoryTransaction transaction);
    }
}
