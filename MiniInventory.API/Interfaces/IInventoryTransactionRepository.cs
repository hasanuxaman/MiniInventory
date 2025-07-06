using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task<IEnumerable<InventoryTransaction>> GetAllAsync();
        Task<IEnumerable<InventoryTransaction>> GetByProductIdAsync(int productId);
        Task<int> AddAsync(InventoryTransaction transaction);
    }
}
