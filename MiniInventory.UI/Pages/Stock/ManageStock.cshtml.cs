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

        public ManageStockModel(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        [BindProperty]
        public List<StockTransaction> Transactions { get; set; } = new();

        public List<SelectListItem> Products { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpFactory.CreateClient("API");
            var products = await client.GetFromJsonAsync<List<Product>>("products");

            if (products != null)
            {
                Products = products.Select(p =>
                    new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpFactory.CreateClient("API");

            foreach (var trx in Transactions)
            {
                if (trx.Type == "IN")
                    await client.PostAsJsonAsync("stock/in", trx);
                else if (trx.Type == "OUT")
                    await client.PostAsJsonAsync("stock/out", trx);
            }

            return RedirectToPage("Index");
        }
    }
}
