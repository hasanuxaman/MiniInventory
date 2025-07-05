namespace MiniInventory.UI.Models
{
    public class StockReport
    {
        public string ProductName { get; set; } = "";
        public int CurrentStock { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }
}
