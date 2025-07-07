using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace MiniInventory.UI.Pages.Stock
{
    public class ManageStockModel : PageModel
    {
        private readonly IHttpClientFactory _httpFactory;

        private readonly IHttpClientFactory _clientFactory;

        public ManageStockModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public StockTransaction Transaction { get; set; }
  
 

        public List<SelectListItem> ProductList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var products = await client.GetFromJsonAsync<List<Product>>("Products/GetProductAll");

            ProductList = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); 
                return Page();
            }

            var client = _clientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("InventoryTransactions/AddStockTransectionl", Transaction);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Stock transaction saved successfully.";
                return RedirectToPage("Index");
            }
            var errorMessage = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = $"{response.StatusCode}: {errorMessage}";
            await OnGetAsync();
            return Page();
        }
    }
}
