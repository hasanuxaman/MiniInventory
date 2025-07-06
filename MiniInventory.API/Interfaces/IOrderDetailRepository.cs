using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId);
        Task<int> AddAsync(OrderDetail detail);
        Task<int> DeleteAsync(int orderId, int productId);
    }
}
