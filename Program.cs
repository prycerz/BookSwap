using Microsoft.EntityFrameworkCore;
using BookSwap.Models; // jeśli AppDbContext jest w Models

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Sesja
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Rejestracja AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=bookswap.db")); // <== To dodaj

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // <== Sesja przed autoryzacją
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // <== Możesz ustawić Login jako domyślną stronę

app.Run();

