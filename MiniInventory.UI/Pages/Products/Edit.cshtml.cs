using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Products
{
    public class EditModel : PageModel
    {

        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [BindProperty]
        public Product Product { get; set; }

        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Product = await _httpClient.GetFromJsonAsync<Product>($"Products/GetProductById?id={id}");
            if (Product == null)
            {
                return NotFound();
            }


            var categories = await _httpClient.GetFromJsonAsync<List<Category>>("Categories/GetCategoryAll");
            CategoryList = new SelectList(categories, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {

                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("Categories/GetCategoryAll");
                CategoryList = new SelectList(categories, "Id", "Name");

                return Page();
            }


            var response = await _httpClient.PutAsJsonAsync($"Products/UpdateProductById?id={id}", Product);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to update product.");
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("Categories/GetCategoryAll");
                CategoryList = new SelectList(categories, "Id", "Name");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
