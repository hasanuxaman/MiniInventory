using MiniInventory.API.Models;

namespace MiniInventory.API.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
      
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";

        public ICollection<OrderDetailDto>? OrderDetails { get; set; }
    }
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Unit { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
