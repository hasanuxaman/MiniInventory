using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public EditModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Customer Customer { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var customer = await client.GetFromJsonAsync<Customer>($"Customers/GetCustomerbyId?id={id}");

            if (customer == null) return RedirectToPage("Index");

            Customer = customer;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _clientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync($"Customers/UpdateCustomerbyId?id={id}", Customer);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Customer updated successfully.";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update customer.");
            return Page();
        }
    }
}
