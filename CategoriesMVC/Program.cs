using CategoriesMVC.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("CategoriesApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:CategoriesApi"]!);
});

builder.Services.AddHttpClient("ProductsApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:ProductsApi"]!);
});

builder.Services.AddHttpClient("AuthenticationApi", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:AuthenticationApi"]!);
    c.DefaultRequestHeaders.Accept.Clear();
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthentication, Authentication>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
