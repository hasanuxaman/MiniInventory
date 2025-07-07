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

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            var sql = @"
        SELECT o.OrderId, o.CustomerId, o.OrderDate, o.Status,
               d.ProductId, d.Quantity, d.UnitPrice
        FROM [Order] o
        LEFT JOIN OrderDetail d ON o.OrderId = d.OrderId
        ORDER BY o.OrderId DESC;";

            var orderDict = new Dictionary<int, Order>();

            var result = await conn.QueryAsync<Order, OrderDetail, Order>(
                sql,
                (order, detail) =>
                {
                    if (!orderDict.TryGetValue(order.OrderId, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.OrderDetails = new List<OrderDetail>();
                        orderDict.Add(order.OrderId, currentOrder);
                    }

                    if (detail != null)
                    {
                        currentOrder.OrderDetails.Add(detail);
                    }

                    return currentOrder;
                },
                splitOn: "ProductId"
            );

            return orderDict.Values;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            using var conn = new SqlConnection(_connectionString);

            
            var orderSql = @"SELECT OrderId, CustomerId, OrderDate, Status
                     FROM [Order]
                     WHERE OrderId = @OrderId";

            var order = await conn.QuerySingleOrDefaultAsync<Order>(orderSql, new { OrderId = orderId });

            if (order == null)
                return null;

            
            var detailSql = @"SELECT ProductId, Quantity, UnitPrice
                      FROM OrderDetail
                      WHERE OrderId = @OrderId";

            var orderDetails = await conn.QueryAsync<OrderDetail>(detailSql, new { OrderId = orderId });

            order.OrderDetails = orderDetails.ToList();

            return order;
        }
        public async Task<int> AddOrderAsync(Order orderDto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                var orderSql = @"INSERT INTO [Order] (CustomerId, OrderDate, Status)
                         VALUES (@CustomerId, @OrderDate, @Status);
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var orderId = await conn.ExecuteScalarAsync<int>(orderSql, new
                {
                    orderDto.CustomerId,
                    orderDto.OrderDate,
                    orderDto.Status
                }, tran);

                var detailSql = @"INSERT INTO OrderDetail (OrderId, ProductId, Quantity, UnitPrice)
                          VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);";

                var updateStockSql = @"UPDATE ProductCurrentStocks
                               SET CurrentStock = CurrentStock - @Quantity
                               WHERE ProductId = @ProductId;";

                foreach (var detail in orderDto.OrderDetails)
                {
                    
                    await conn.ExecuteAsync(detailSql, new
                    {
                        OrderId = orderId,
                        detail.ProductId,
                        detail.Quantity,
                        detail.UnitPrice
                    }, tran);

                    
                    await conn.ExecuteAsync(updateStockSql, new
                    {
                        detail.ProductId,
                        detail.Quantity
                    }, tran);
                }

                await tran.CommitAsync();
                return orderId;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(int orderId, Order orderDto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                
                var updateOrderSql = @"UPDATE [Order]
                               SET CustomerId = @CustomerId,
                                   OrderDate = @OrderDate,
                                   Status = @Status
                               WHERE OrderId = @OrderId;";

                await conn.ExecuteAsync(updateOrderSql, new
                {
                    OrderId = orderId,
                    orderDto.CustomerId,
                    orderDto.OrderDate,
                    orderDto.Status
                }, tran);

                
                var deleteDetailSql = "DELETE FROM OrderDetail WHERE OrderId = @OrderId";
                await conn.ExecuteAsync(deleteDetailSql, new { OrderId = orderId }, tran);

                
                var insertDetailSql = @"INSERT INTO OrderDetail (OrderId, ProductId, Quantity, UnitPrice)
                                VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice);";

                foreach (var detail in orderDto.OrderDetails)
                {
                    await conn.ExecuteAsync(insertDetailSql, new
                    {
                        OrderId = orderId,
                        detail.ProductId,
                        detail.Quantity,
                        detail.UnitPrice
                    }, tran);
                }

                await tran.CommitAsync();
                return true;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }


        public async Task<int> DeleteAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteAsync("DELETE FROM [Order] WHERE OrderId = @Id", new { Id = id });
        }

        
    }


}
