using MiniInventory.API.Data;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace MiniInventory.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT p.Id
      ,p.Name
      ,p.UnitPrice
      ,p.CategoryId
      ,p.Unit
	  ,c.Name as CategoryName
	  
  FROM Products p
  left join Categories  c  on p.CategoryId=c.Id";
            return await connection.QueryAsync<ProductDto>(sql);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Products WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task AddAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Products (Name, Unit,UnitPrice, CategoryId) VALUES (@Name, @Unit, @UnitPrice, @CategoryId)";
            await connection.ExecuteAsync(sql, product);
        }

        public async Task UpdateAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Products SET Name=@Name,Unit=@Unit,  UnitPrice=@UnitPrice, CategoryId=@CategoryId WHERE Id=@Id";
            await connection.ExecuteAsync(sql, product);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Products WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
