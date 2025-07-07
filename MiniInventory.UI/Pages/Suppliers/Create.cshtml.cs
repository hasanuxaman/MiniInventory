using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Suppliers
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var client = _clientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("Suppliers/AddSupplier", Supplier);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Supplier added successfully.";
                return RedirectToPage("/Suppliers/Create");
            }

            ModelState.AddModelError(string.Empty, "Failed to add supplier.");
            return Page();
        }
    }
}
