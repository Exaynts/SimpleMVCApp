using MvcApp.Models; // Чтобы использовать тип Product
using System.Collections.Generic; // Для использования коллекций, например IEnumerable

namespace MvcApp.Repositories
{
    // Интерфейс называется IProductRepository (I - стандартное обозначение интерфейса)
    public interface IProductRepository
    {
        // Получить все товары. IEnumerable позволяет проходиться по списку (например, в цикле foreach)
        IEnumerable<Product> GetAll();
        // Получить один товар по его Id. Может вернуть null, если товар не найден.
        Product? GetById(int id);
        // Добавить новый товар. Обычно в реализациях здесь ему присваивается Id.
        void Add(Product product);
        // Обновить существующий товар.
        void Update(Product product);
        // Удалить товар по Id.
        void Delete(int id);
        // Получить товары определенной категории.
        IEnumerable<Product> GetByCategory(string category);
        // Получить только те товары, что есть в наличии (InStock == true).
        IEnumerable<Product> GetInStock();

        //Новые методы для LINQ запросов
        // Фильтрация по цене
        IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        // Топ N самых дорогих товаров
        IEnumerable<Product> GetTopExpensiveProducts(int count);
        // Поиск по тексту
        IEnumerable<Product> SearchProducts(string searchTerm);
        // Статистика
        decimal GetAveragePrice();
        int GetTotalCount();
        (decimal MinPrice, decimal MaxPrice) GetPriceRange();
        bool AnyInCategory(string category);
        // Группировка
        IEnumerable<IGrouping<string, Product>> GetProductsGroupedByCategory();
        // Пагинация
        IEnumerable<Product> GetProductsWithPagination(int page, int pageSize);
        int GetTotalPages(int pageSize);

        // Асинхронные версии
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice,
        decimal maxPrice);
        Task<decimal> GetAveragePriceAsync();
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<IGrouping<string, Product>>>
        GetProductsGroupedByCategoryAsync();
    }
}