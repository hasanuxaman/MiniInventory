using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public EditModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Supplier Supplier { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var result = await client.GetFromJsonAsync<Supplier>($"Suppliers/GetSupplierById?id={id}");

            if (result == null)
                return RedirectToPage("Index");

            Supplier = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _clientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync($"Suppliers/UpdateSupplierById?id={id}", Supplier);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Supplier updated successfully.";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update supplier.");
            return Page();
        }
    }
}
