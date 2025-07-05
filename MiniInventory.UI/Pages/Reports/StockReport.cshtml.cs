using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Reports
{
    public class StockReportModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public List<StockReport> ReportList { get; set; } = new();

        public StockReportModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");
            ReportList = await client.GetFromJsonAsync<List<StockReport>>("reports/stock");
        }
    }
}
