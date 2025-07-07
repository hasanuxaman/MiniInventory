namespace MiniInventory.API.DTOs
{
    public class StockReportDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public int CurrentStock { get; set; }
        public decimal UnitPrice { get; set; }
        public string Unit { get; set; }
    }
}
