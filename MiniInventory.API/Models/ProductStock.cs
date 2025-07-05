namespace MiniInventory.API.Models
{
    public class ProductStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string TransactionType { get; set; } = string.Empty; // "IN" or "OUT"
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public Product? Product { get; set; }
    }
}
