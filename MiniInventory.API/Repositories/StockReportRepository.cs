using Dapper;
using Microsoft.Data.SqlClient;
using MiniInventory.API.DTOs;
using MiniInventory.API.Interfaces;

namespace MiniInventory.API.Repositories
{
    public class StockReportRepository : IStockReportRepository
    {
        private readonly string _connectionString;

        public StockReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<StockReportDto>> GetStockReportAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                      SELECT 
               p.Id AS ProductId,
               p.Name AS ProductName,
               ISNULL(s.CurrentStock, 0) AS CurrentStock,
               p.UnitPrice,
               p.Unit,
               c.Name as CategoryName
           FROM Products p
           LEFT JOIN ProductCurrentStocks s ON p.Id = s.ProductId
LEFT JOIN Categories c ON p.CategoryId = c.Id
           ORDER BY p.Id;
        ";

            var result = await connection.QueryAsync<StockReportDto>(sql);
            return result;
        }
    }
}
