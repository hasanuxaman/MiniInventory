var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7004/api/");
});

// Uncomment & configure if JWT Authentication needed for API calls
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//             ValidAudience = builder.Configuration["JwtSettings:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
//         };
//     });

builder.Services.AddAuthorization();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();      
app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

  
    if (path != null && !path.StartsWith("/login") && !path.StartsWith("/css") && !path.StartsWith("/js") && !path.StartsWith("/lib"))
    {
        var token = context.Session.GetString("JWT");
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Redirect("/Login");
            return;
        }
    }
    await next();
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();
