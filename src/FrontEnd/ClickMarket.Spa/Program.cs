using ClickMarket.Spa.Components;
using ClickMarket.Spa.Security;
using ClickMarket.Spa.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddHttpClient("ClickMarketAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7251/api/");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddScheme<CustomOptions, JWTAuthenticationHandler>(
        "JWTAuth", options => { }
    );
builder.Services.AddScoped<JWTAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
