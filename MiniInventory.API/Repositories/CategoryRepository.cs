using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using System.Data;
using Dapper;

namespace MiniInventory.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _dbConnection;

       
        public CategoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Category> AddAsync(Category category)
        {
            string sql = @"
                INSERT INTO Categories (Name)
                VALUES (@Name );
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var id = await _dbConnection.QuerySingleAsync<int>(sql, new
            {
                category.Name,
                
            });

            category.Id = id;
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "DELETE FROM Categories WHERE Id = @Id";

            int rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            string sql = "SELECT * FROM Categories";

            var categories = await _dbConnection.QueryAsync<Category>(sql);

            return categories;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            string sql = "SELECT * FROM Categories WHERE Id = @Id";

            var category = await _dbConnection.QuerySingleOrDefaultAsync<Category>(sql, new { Id = id });

            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            string sql = @"
                UPDATE Categories
                SET Name = @Name
                   
                WHERE Id = @Id
            ";

            await _dbConnection.ExecuteAsync(sql, new
            {
                category.Name,
               
                category.Id
            });
        }
    }
}
