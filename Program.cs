using FamilyLibrary.Persistence;
using FamilyLibrary.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllersWithViews(o =>
    o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<BooksRepository>();
builder.Services.AddScoped<BooksService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Books/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Books/Error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
