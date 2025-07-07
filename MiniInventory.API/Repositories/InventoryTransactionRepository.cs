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

        public async Task<IEnumerable<InventoryTransactionDto>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<InventoryTransactionDto>(@" SELECT 
    st.Id,
    st.ProductId,
    p.Name AS ProductName,
    st.Quantity,
    st.Type,
    st.TransactionDate
FROM StockTransactions st
Left JOIN Products p ON st.ProductId = p.Id

ORDER BY st.TransactionDate DESC");
        }

        public async Task<IEnumerable<InventoryTransaction>> GetByProductIdAsync(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM StockTransactions WHERE ProductId = @ProductId";
            return await conn.QueryAsync<InventoryTransaction>(sql, new { ProductId = productId });
        }

        public async Task<(bool IsSuccess, string Message)> AddAsync(InventoryTransaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transactionScope = connection.BeginTransaction();

            try
            {
                var insertQuery = @"INSERT INTO StockTransactions (ProductId, Quantity, TransactionDate, Type)
                    VALUES (@ProductId, @Quantity, @TransactionDate, @Type)";
                await connection.ExecuteAsync(insertQuery, transaction, transactionScope);

                var stockQuery = "SELECT * FROM ProductCurrentStocks WHERE ProductId = @ProductId";
                var currentStock = await connection.QueryFirstOrDefaultAsync<ProductCurrentStocks>(stockQuery, new { transaction.ProductId }, transactionScope);

                int newStock = 0;

                if (transaction.Type == "IN")
                {
                    newStock = (currentStock?.CurrentStock ?? 0) + transaction.Quantity;
                }
                else if (transaction.Type == "OUT")
                {
                    if ((currentStock?.CurrentStock ?? 0) < transaction.Quantity)
                        return (false, "Not enough stock to transfer.");

                    newStock = currentStock.CurrentStock - transaction.Quantity;
                }

                var existing = await connection.QueryFirstOrDefaultAsync<int?>(
                    "SELECT Id FROM ProductCurrentStocks WHERE ProductId = @ProductId",
                    new { ProductId = transaction.ProductId },
                    transactionScope);

                if (existing.HasValue)
                {
                    var updateQuery = @"UPDATE ProductCurrentStocks 
                                SET CurrentStock = @CurrentStock 
                                WHERE ProductId = @ProductId";

                    await connection.ExecuteAsync(updateQuery, new
                    {
                        ProductId = transaction.ProductId,
                        CurrentStock = newStock
                    }, transactionScope);
                }
                else
                {
                    var insertstockQuery = @"INSERT INTO ProductCurrentStocks (ProductId, CurrentStock)
                                     VALUES (@ProductId, @CurrentStock)";

                    await connection.ExecuteAsync(insertstockQuery, new
                    {
                        ProductId = transaction.ProductId,
                        CurrentStock = newStock
                    }, transactionScope);
                }

                await transactionScope.CommitAsync();
                return (true, "Stock transaction successful.");
            }
            catch (Exception ex)
            {
                await transactionScope.RollbackAsync();
                return (false, $"Error: {ex.Message}");
            }
        }
    }

}
