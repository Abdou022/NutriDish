using NutriDish.Components;
using NutriDish.Models;
using NutriDish.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Get the connection string from appsettings.json and configure the DbContext to use SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// This registers the AppDbContext with the dependency injection system, so it can be injected into components and other services that need to access the database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
//================================================================================

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
