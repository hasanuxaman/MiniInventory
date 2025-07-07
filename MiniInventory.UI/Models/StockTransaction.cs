using System.ComponentModel.DataAnnotations;

namespace MiniInventory.UI.Models
{
   
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public string? ProductName { get; set; } = "";
        //public decimal ProductCurrentStock { get; set; } = 0;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        public string Type { get; set; } = ""; // or "OUT"
        public DateTime TransactionDate { get; set; } = DateTime.Now;



    }
}
