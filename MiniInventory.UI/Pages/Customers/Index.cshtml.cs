using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<Customer> Customers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var result = await client.GetFromJsonAsync<List<Customer>>("Customers/GetAllCustomer"); 

            if (result != null)
            {
                Customers = result;
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"Customers/DeleteCustomerById?id={id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Customer deleted successfully.";
            }
            else
            {
                TempData["Message"] = "Failed to delete customer.";
            }

            return RedirectToPage();
        }
    }
}
