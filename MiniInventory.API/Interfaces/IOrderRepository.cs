using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<int> AddAsync(Order order);
        Task<int> UpdateAsync(Order order);
        Task<int> DeleteAsync(int id);
    }
}
