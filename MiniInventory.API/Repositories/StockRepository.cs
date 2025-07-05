using MiniInventory.API.Interfaces;
using MiniInventory.API.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MiniInventory.API.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly string _connectionString;

        public StockRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<StockTransaction>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM ProductStocks ORDER BY TransactionDate DESC";
            var result = await connection.QueryAsync<StockTransaction>(sql);
            return result;
        }

        public async Task<StockTransaction?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM ProductStocks WHERE Id = @id";
            var result = await connection.QueryFirstOrDefaultAsync<StockTransaction>(sql, new { id });
            return result;
        }

        public async Task AddAsync(StockTransaction stock)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO ProductStocks (ProductId, Quantity, TransactionDate, Type)
                        VALUES (@ProductId, @Quantity, @TransactionDate, @Type)";
            await connection.ExecuteAsync(sql, stock);
        }

        public async Task UpdateAsync(StockTransaction stock)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE ProductStocks 
                        SET ProductId = @ProductId, Quantity = @Quantity, 
                            TransactionDate = @TransactionDate, Type = @Type 
                        WHERE Id = @Id";
            await connection.ExecuteAsync(sql, stock);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM ProductStocks WHERE Id = @id";
            await connection.ExecuteAsync(sql, new { id });
        }
        public async Task<int> GetCurrentStockAsync(int productId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT ISNULL(Quantity, 0) FROM ProductStocks WHERE ProductId = @productId";
            var stock = await connection.ExecuteScalarAsync<int>(sql, new { productId });
            return stock;
        }
        public async Task StockInAsync(int productId, int qty)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
               
                var existingQty = await connection.ExecuteScalarAsync<int?>(
                    "SELECT Quantity FROM ProductStocks WHERE ProductId = @productId",
                    new { productId }, transaction);

                if (existingQty.HasValue)
                {
                    
                    string updateStock = "UPDATE ProductStocks SET Quantity = Quantity + @qty WHERE ProductId = @productId";
                    await connection.ExecuteAsync(updateStock, new { qty, productId }, transaction);
                }
                else
                {
                    
                    string insertStock = @"INSERT INTO ProductStocks (ProductId, Quantity, TransactionDate, TransactionType) 
                                   VALUES (@productId, @qty, @date, 'IN')";
                    await connection.ExecuteAsync(insertStock, new { productId, qty, date = DateTime.UtcNow }, transaction);
                }

               
                string updateProduct = "UPDATE Products SET Quantity = Quantity + @qty WHERE Id = @productId";
                await connection.ExecuteAsync(updateProduct, new { qty, productId }, transaction);

                
                string insertTxn = @"INSERT INTO StockTransactions (ProductId, Quantity, TransactionDate, Type) 
                             VALUES (@productId, @qty, @date, 'IN')";
                await connection.ExecuteAsync(insertTxn, new { productId, qty, date = DateTime.UtcNow }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public async Task StockOutAsync(int productId, int qty)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var currentQty = await connection.ExecuteScalarAsync<int?>(
                    "SELECT Quantity FROM ProductStocks WHERE ProductId = @productId",
                    new { productId }, transaction);

                if (!currentQty.HasValue || currentQty.Value < qty)
                    throw new Exception("Insufficient stock quantity");

                
                string updateStock = "UPDATE ProductStocks SET Quantity = Quantity - @qty WHERE ProductId = @productId";
                await connection.ExecuteAsync(updateStock, new { qty, productId }, transaction);

                
                string updateProduct = "UPDATE Products SET Quantity = Quantity - @qty WHERE Id = @productId";
                await connection.ExecuteAsync(updateProduct, new { qty, productId }, transaction);

                
                string insertTxn = @"INSERT INTO StockTransactions (ProductId, Quantity, TransactionDate, Type) 
                             VALUES (@productId, @qty, @date, 'OUT')";
                await connection.ExecuteAsync(insertTxn, new { productId, qty, date = DateTime.UtcNow }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}
