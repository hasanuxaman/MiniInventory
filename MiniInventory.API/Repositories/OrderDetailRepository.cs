using Dapper;
using Microsoft.Data.SqlClient;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly string _connectionString;

        public OrderDetailRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM order_details WHERE order_id = @OrderId";
            return await conn.QueryAsync<OrderDetail>(sql, new { OrderId = orderId });
        }

        public async Task<int> AddAsync(OrderDetail detail)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO order_details (order_id, product_id, quantity, unit_price)
                    VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)";
            return await conn.ExecuteAsync(sql, detail);
        }

        public async Task<int> DeleteAsync(int orderId, int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"DELETE FROM order_details WHERE order_id = @OrderId AND product_id = @ProductId";
            return await conn.ExecuteAsync(sql, new { OrderId = orderId, ProductId = productId });
        }
    }

}
