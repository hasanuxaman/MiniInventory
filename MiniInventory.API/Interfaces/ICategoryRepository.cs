using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}
