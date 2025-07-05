namespace MiniInventory.UI.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductName { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = "IN"; // or "OUT"
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
