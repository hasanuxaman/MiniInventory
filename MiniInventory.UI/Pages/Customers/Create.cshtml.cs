using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public Customer Customer { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient("API");

            var response = await client.PostAsJsonAsync("Customers/AddCustomer", Customer);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Customer added successfully.";
                return RedirectToPage("/Customers/Create");
            }

            ModelState.AddModelError(string.Empty, "Failed to add customer.");
            return Page();
        }
    }
}
