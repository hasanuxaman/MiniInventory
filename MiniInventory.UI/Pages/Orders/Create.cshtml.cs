using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;

namespace MiniInventory.UI.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [BindProperty]
        public Order Order { get; set; } = new();

        public List<Product> Products { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Order = new Order
            {
                OrderDate = DateTime.Today,
                Status = "Approved"
            };
            
            Products = await _httpClient.GetFromJsonAsync<List<Product>>("Products/GetProductAll") ?? new();
            Customers = await _httpClient.GetFromJsonAsync<List<Customer>>("Customers/GetAllCustomer") ?? new();

            if (Order.OrderDetails == null || !Order.OrderDetails.Any())
            {
                Order.OrderDetails = new List<OrderDetail> { new OrderDetail() };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("Orders/AddOrder", Order);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Order creation failed.");
            await OnGetAsync(); 
            return Page();
        }
    }
}
