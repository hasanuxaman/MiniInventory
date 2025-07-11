﻿using Microsoft.AspNetCore.Mvc;
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

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<Order> Orders { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _httpClient.GetFromJsonAsync<List<Order>>("Orders/GetAllOrder");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Orders/DeleteOrderById?id={Id}");

            if (response.IsSuccessStatusCode)
                return RedirectToPage(); 

            ModelState.AddModelError(string.Empty, "Delete failed.");
            return Page();
        }

    }
}
