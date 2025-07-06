using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MiniInventory.API.Repositories
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly string _connectionString;

        public InventoryTransactionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<InventoryTransaction>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<InventoryTransaction>("SELECT * FROM inventory_transactions");
        }

        public async Task<IEnumerable<InventoryTransaction>> GetByProductIdAsync(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM inventory_transactions WHERE product_id = @ProductId";
            return await conn.QueryAsync<InventoryTransaction>(sql, new { ProductId = productId });
        }

        public async Task<int> AddAsync(InventoryTransaction transaction)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO inventory_transactions (product_id, transaction_type, quantity, transaction_date, notes)
                    VALUES (@ProductId, @TransactionType, @Quantity, @TransactionDate, @Notes)";
            return await conn.ExecuteAsync(sql, transaction);
        }
    }

}
