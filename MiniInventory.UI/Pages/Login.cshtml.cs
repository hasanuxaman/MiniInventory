using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniInventory.UI.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            if (HttpContext.Session.GetString("JWT") != null)
            {
                Response.Redirect("/Index");
            }
        }
        
        private readonly IHttpContextAccessor _accessor;
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _accessor = accessor;
        }




        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
           
            var response = await _httpClient.PostAsJsonAsync("Auth/login", new { Username, Password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                _accessor.HttpContext.Session.SetString("JWT", result.Token);
                return RedirectToPage("/Index");
            }

            Message = "Login failed";
            return Page();
        }
        

        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
