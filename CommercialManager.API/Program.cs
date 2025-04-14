using CommercialManager.API.Database;
using CommercialManager.API.Helpers;
using CommercialManager.API.Services;
using CommercialManager.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add database
builder.Services.AddDbContext<CommercialDbContext>(option =>
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

// ------ Services ------

builder.Services.AddTransient<ICategoriesServices, CategoriesServices>();
builder.Services.AddTransient<IProductsServices, ProductsServices>();
builder.Services.AddTransient<IShoppingCartsServices, ShoppingCartsServices>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
