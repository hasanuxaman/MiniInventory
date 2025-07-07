namespace MiniInventory.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }
     
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }

}
