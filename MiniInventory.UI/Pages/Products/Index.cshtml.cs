using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace MiniInventory.UI.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }
        public List<Product> Products { get; set; } = [];

        public async Task OnGetAsync()
        {
  
             Products = await _httpClient.GetFromJsonAsync <List<Product>> ("Products/GetProductAll") ?? new();


        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Products/DeleteProductById?id={id}");
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Delete failed");
            return Page();
        }
    }
}
