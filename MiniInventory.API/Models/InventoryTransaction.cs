namespace MiniInventory.API.Models
{
    public class InventoryTransaction
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
       
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = ""; // "IN" or "OUT"
    }
    public class InventoryTransactionDto
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
       // public int ProductCurrentStock { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = ""; // "IN" or "OUT"
    }
}
