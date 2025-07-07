using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<int> AddAsync(Supplier supplier);
        Task<int> UpdateAsync(int id,Supplier supplier);
        Task<int> DeleteAsync(int id);
    }
}
