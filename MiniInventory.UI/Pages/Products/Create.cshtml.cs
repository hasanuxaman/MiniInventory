using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniInventory.UI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    [BindProperty]
    public Product Product { get; set; } = new(); 

    public List<Category> Categories { get; set; } = new();

    public CreateModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("API");
        var categories = await client.GetFromJsonAsync<List<Category>>("Categories/GetCategoryAll");
        if (categories != null)
            Categories = categories;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var client = _httpClientFactory.CreateClient("API");
        var response = await client.PostAsJsonAsync("Products/AddProduct", Product);
        if (response.IsSuccessStatusCode)
            return RedirectToPage("Index");

        ModelState.AddModelError(string.Empty, "Failed to create product.");
        await OnGetAsync(); 
        return Page();
    }
}
