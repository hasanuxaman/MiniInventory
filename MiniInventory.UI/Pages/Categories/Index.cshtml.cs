using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Categories
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

        public List<Category> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _httpClient.GetFromJsonAsync<List<Category>>("Categories/GetCategoryAll") ?? new();
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var response = await _httpClient.DeleteAsync($"Categories/DeleteCategoryById?id={Id}");
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Delete failed");
            return Page();
        }
    }
}
