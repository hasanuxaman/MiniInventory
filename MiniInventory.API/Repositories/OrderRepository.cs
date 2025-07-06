using Dapper;
using Microsoft.Data.SqlClient;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Order>("SELECT * FROM [Order]");
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Order>("SELECT * FROM [Order] WHERE OrderId = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Order order)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [Order] (CustomerId, OrderDate, Status)
                VALUES (@CustomerId, @OrderDate, @Status);
                SELECT CAST(SCOPE_IDENTITY() as int);";
            return await conn.ExecuteScalarAsync<int>(sql, order);
        }

        public async Task<int> UpdateAsync(Order order)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"UPDATE [Order] SET 
                        CustomerId = @CustomerId,
                        OrderDate = @OrderDate,
                        Status = @Status
                    WHERE OrderId = @OrderId";
            return await conn.ExecuteAsync(sql, order);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync("DELETE FROM [Order] WHERE OrderId = @Id", new { Id = id });
        }
    }


}
