namespace MiniInventory.UI.Models
{
    public class OrderDetail
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
