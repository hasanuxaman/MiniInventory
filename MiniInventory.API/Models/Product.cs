namespace MiniInventory.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
       
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public int CategoryId { get; set; }
        
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

    }
}
