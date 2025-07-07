using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Order Order { get; set; } = new();

        public List<Product> Products { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            
            Order = await _httpClient.GetFromJsonAsync<Order>($"Orders/GetOrderById?id={Id}") ?? new Order();
            Order.OrderDetails ??= new List<OrderDetail>();

          
            Products = await _httpClient.GetFromJsonAsync<List<Product>>("Products/GetProductAll") ?? new();
            Customers = await _httpClient.GetFromJsonAsync<List<Customer>>("Customers/GetAllCustomer") ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PutAsJsonAsync($"Orders/UpdateOrderbyId?id={Id}", Order);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Order update failed.");
            await OnGetAsync(); 
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var response = await _httpClient.DeleteAsync($"Orders/DeleteOrderById?id={Id}");

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Order deletion failed.");
            await OnGetAsync(); 
            return Page();
        }
    }
}
