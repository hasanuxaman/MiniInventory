using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Suppliers
{
    public class IndexModel : PageModel
    {
       
        
            private readonly IHttpClientFactory _clientFactory;

            public IndexModel(IHttpClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public List<Supplier> Suppliers { get; set; } = new();

            public async Task OnGetAsync()
            {
                var client = _clientFactory.CreateClient("API");
                var result = await client.GetFromJsonAsync<List<Supplier>>("Suppliers/GetAllSupplier"); 

                if (result != null)
                {
                    Suppliers = result;
                }
            }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"Suppliers/DeleteSupplierById?id={id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Supplier deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Failed to delete supplier.";
            }

            return RedirectToPage();
        }
    }
}
