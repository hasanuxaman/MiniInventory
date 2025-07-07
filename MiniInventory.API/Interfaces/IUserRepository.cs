using MiniInventory.API.Models;

namespace MiniInventory.API.Interfaces
{
    public interface IUserRepository
    {
        UserLogin? ValidateUser(string username, string password);
    }
}
