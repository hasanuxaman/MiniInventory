using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        
        private static readonly List<UserLogin> users = new()
    {
        new UserLogin { Username = "admin", Password = "admin123" }
    };

        public UserLogin? ValidateUser(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
