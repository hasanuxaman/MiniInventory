using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;
using System.Text.Json;

namespace MiniInventory.UI.Pages.Products
{
    public class IndexModel : PageModel
    {
        public List<Product> Products { get; set; } = [];

        public async Task OnGetAsync()
        {
            using var httpClient = new HttpClient();

            string apiUrl = "https://localhost:7004/api/Products/GetProductAll";

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    }
}
