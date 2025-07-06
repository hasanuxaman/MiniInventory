using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

     
        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _httpClient.GetFromJsonAsync<List<Order>>("Orders/GetAllOrder");
        }
    }
}
