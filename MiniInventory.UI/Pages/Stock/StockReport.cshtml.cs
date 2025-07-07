using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Stock
{
    public class StockReportModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public StockReportModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API"); 
        }

        public List<StockReport> StockReports { get; set; }

        public async Task OnGetAsync()
        {
            StockReports = await _httpClient.GetFromJsonAsync<List<StockReport>>("StockReport/GetStockReport");
        }
    }
}
