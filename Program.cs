using Microsoft.EntityFrameworkCore;
using MvcApp.Data;
using MvcApp.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Добавление MVC
builder.Services.AddControllersWithViews();

// Устанавливаем папку App_Data как DataDirectory для LocalDB
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "App_Data"));

// Регистрация контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());

// Регистрация репозиториев (EF Core)
builder.Services.AddScoped<ICourseRepository, EfCourseRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();

var app = builder.Build();

// Инициализация базы данных тестовыми данными (SeedData)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    CourseSeedData.Initialize(dbContext);   // добавление курсов
    ProductSeedData.Initialize(dbContext);  // добавление товаров
}

// Конфигурация конвейера обработки запросов (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Кастомные маршруты
app.MapControllerRoute(
    name: "about",
    pattern: "about-us",
    defaults: new { controller = "Home", action = "Privacy" });

app.MapControllerRoute(
    name: "userProfile",
    pattern: "user/{username}/{action=Profile}",
    defaults: new { controller = "Demo" });

app.MapControllerRoute(
    name: "product",
    pattern: "product/{id:int}",
    defaults: new { controller = "Demo", action = "ProductDetails" });

app.MapControllerRoute(
    name: "courses",
    pattern: "Course/{action=Index}/{id?}",
    defaults: new { controller = "Courses" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();