using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<int> AddOrderAsync(Order order);
        Task<bool> UpdateAsync(int orderId, Order orderDto);
        Task<int> DeleteAsync(int id);
    }
}
