using Dapper;
using Microsoft.Data.SqlClient;
using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;

namespace MiniInventory.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Customer>("SELECT * FROM Customer");
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Customer>(
                "SELECT * FROM Customer WHERE CustomerId = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Customer customer)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Customer (CustomerName, email, phone, address)
                    VALUES (@CustomerName, @Email, @Phone, @Address)";
            return await conn.ExecuteAsync(sql, customer);
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"UPDATE Customer SET 
                        CustomerName = @CustomerName,
                        email = @Email,
                        phone = @Phone,
                        address = @Address
                    WHERE CustomerId = @CustomerId";
            return await conn.ExecuteAsync(sql, customer);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync("DELETE FROM Customer WHERE CustomerId = @Id", new { Id = id });
        }
    }

}
