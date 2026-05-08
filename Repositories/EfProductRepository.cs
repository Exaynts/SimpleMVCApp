using Microsoft.EntityFrameworkCore;
using MvcApp.Data;
using MvcApp.Models;

namespace MvcApp.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly AppDbContext context;
        public EfProductRepository(AppDbContext _context)
        {
            context = _context;
        }
        public IEnumerable<Product> GetAll()
        {
            return context.Products.ToList();
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }
        public Product? GetById(int id)
        {
            return context.Products.Find(id);
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }
        public void Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }
        public async Task AddAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        public void Update(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
        }
        public async Task UpdateAsync(Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }
        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
        }
        public IEnumerable<Product> GetByCategory(string category)
        {
            return context.Products
            .Where(p => p.Category == category)
            .ToList();
        }
        public IEnumerable<Product> GetInStock()
        {
            return context.Products
            .Where(p => p.InStock)
            .ToList();
        }

        // ========== НОВЫЕ LINQ-МЕТОДЫ ==========
        /// Фильтрация товаров по диапазону цен
        public IEnumerable<Product> GetProductsByPriceRange(decimal minPrice,
            decimal maxPrice) => context.Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice)
        .OrderBy(p => p.Price)
        .ToList();

        /// Получение топ N самых дорогих товаров
        public IEnumerable<Product> GetTopExpensiveProducts(int count) =>
        context.Products.OrderByDescending(p => p.Price)
        .Take(count).ToList();

        /// Поиск товаров по названию, описанию и категории
        public IEnumerable<Product> SearchProducts(string searchTerm) =>
        context.Products.Where(p => p.Name.Contains(searchTerm) ||
        p.Description.Contains(searchTerm) ||
        p.Category.Contains(searchTerm))
        .OrderBy(p => p.Name).ToList();

        /// Средняя цена всех товаров
        public decimal GetAveragePrice() =>
        context.Products.Average(p => p.Price);

        /// Общее количество товаров
        public int GetTotalCount() => context.Products.Count();

        /// Диапазон цен (минимальная и максимальная)
        public (decimal MinPrice, decimal MaxPrice) GetPriceRange()
        {
            return (
            MinPrice: context.Products.Min(p => p.Price),
            MaxPrice: context.Products.Max(p => p.Price)
            );
        }

        /// Проверка наличия товаров в указанной категории
        public bool AnyInCategory(string category) =>
        context.Products.Any(p => p.Category == category);

        /// Группировка товаров по категориям
        public IEnumerable<IGrouping<string, Product>>
        GetProductsGroupedByCategory() =>
        context.Products
        .GroupBy(p => p.Category)
        .OrderBy(g => g.Key)
        .ToList();

        /// Пагинация: получение товаров для указанной страницы
        public IEnumerable<Product> GetProductsWithPagination(int page, int
        pageSize) =>
        context.Products.OrderBy(p => p.Id)
        .Skip((page - 1) * pageSize) // Пропустить n элементов (сколько страниц "пролистали")
        .Take(pageSize) // Взять k элементов (сколько элементов на странице)
        .ToList();

        /// Общее количество страниц
        public int GetTotalPages(int pageSize)
        {
            var totalCount = GetTotalCount();
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        // ========== АСИНХРОННЫЕ МЕТОДЫ ==========
        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal
        minPrice, decimal maxPrice) =>
        await context.Products
        .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
        .OrderBy(p => p.Price).ToListAsync();

        public async Task<decimal> GetAveragePriceAsync() =>
        await context.Products.AverageAsync(p => p.Price);

        public async Task<int> GetTotalCountAsync() =>
        await context.Products.CountAsync();

        public async Task<IEnumerable<IGrouping<string, Product>>>
        GetProductsGroupedByCategoryAsync() =>
        await context.Products
        .GroupBy(p => p.Category)
        .OrderBy(g => g.Key)
        .ToListAsync();
    }
}