using Dapper;
using Microsoft.Data.SqlClient;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Supplier";
            return await conn.QueryAsync<Supplier>(sql);
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Supplier WHERE SupplierId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Supplier>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Supplier supplier)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Supplier (SupplierName, ContactName, Phone, Email)
                        VALUES (@SupplierName, @ContactName, @Phone, @Email)";
            return await conn.ExecuteAsync(sql, supplier);
        }

        public async Task<int> UpdateAsync(Supplier supplier)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"UPDATE Supplier SET 
                            SupplierName = @SupplierName, 
                            ContactName = @ContactName, 
                            Phone = @Phone, 
                            Email = @Email 
                        WHERE SupplierId = @SupplierId";
            return await conn.ExecuteAsync(sql, supplier);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Supplier WHERE SupplierId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
