namespace MiniInventory.API.Models
{
    public class StockTransaction
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = ""; // "IN" or "OUT"
    }
}
