using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Stock
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<StockTransaction> Transactions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("API");

            var result = await client.GetFromJsonAsync<List<StockTransaction>>("stock");

            if (result != null)
                Transactions = result;
        }
    }
}
