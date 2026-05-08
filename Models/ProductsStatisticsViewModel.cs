
using System.Collections.Generic;
namespace MvcApp.Models
{
    /// ViewModel для страницы статистики товаров.
    /// Содержит все данные, необходимые для отображения статистики.
    public class ProductsStatisticsViewModel
    {
        /// Общее количество товаров
        public int TotalCount { get; set; }
        /// Средняя цена всех товаров
        public decimal AveragePrice { get; set; }
        /// Количество товаров в наличии
        public int InStockCount { get; set; }
        /// Диапазон цен (минимальная и максимальная)
        public (decimal MinPrice, decimal MaxPrice) PriceRange { get; set; }
        /// Список категорий со статистикой по каждой
        public IEnumerable<CategoryStatViewModel> ?Categories { get; set; }
    }
    /// ViewModel для статистики по категории
    public class CategoryStatViewModel
    {
        /// Название категории
        public string ?Category { get; set; }
        /// Количество товаров в категории
        public int Count { get; set; }
        /// Средняя цена в категории
        public decimal AveragePrice { get; set; }
        /// Минимальная цена в категории
        public decimal MinPrice { get; set; }
        /// Максимальная цена в категории
        public decimal MaxPrice { get; set; }
    }
}