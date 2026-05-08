using Microsoft.AspNetCore.Mvc;
using MvcApp.Repositories;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository repository;

        //Внедрение зависимости через конструктор
        public ProductsController(IProductRepository _repository)
        {
            repository = _repository;
        }

        //GET: /Products
        public IActionResult Index()
        {
            var products = repository.GetAll();
            return View(products);
        }

        // GET: /Products/Details/5
        public IActionResult Details(int id)
        {
            var product = repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: /Products/Create
        // Действие для отображения пустой формы создания товара.
        public IActionResult Create()
        {
            return View(); // Отдаем пустое представление Create.cshtml,
                           // в которое будет встроена форма
        }

        //POST: /Products/Create
        // Действие для обработки данных, отправленных с формы создания.
        [HttpPost] // Означает, что этот метод обрабатывает только POST-запросы
        [ValidateAntiForgeryToken] // Защита от межсайтовой подделки запроса (CSRF)

        public IActionResult Create(Product product)
        {
            // Проверяем, прошла ли модель валидацию (атрибуты из шага 1).
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                repository.Add(product);
                TempData["SuccessMessage"] = "Товар успешно добавлен!";
                return RedirectToAction(nameof(Index));
                // Перенаправляем пользователя на список товаров (Index)
            }
            return View(product);
        }

        //GET: /Products/Edit/5
        // Отображает форму редактирования, предварительно заполненную данными товара.
        public IActionResult Edit(int id)
        {
            var product = repository.GetById(id);
            if (product == null) return NotFound();
            return View(product);  // Передаем товар в представление Edit.cshtml          
        }

        //POST: /Products/Edit
        // Обрабатывает данные, отправленные с формы редактирования.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id) // Проверка, совпадает ли Id в маршруте с Id в модели
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    repository.Update(product);
                    TempData["SuccessMessage"] = "Товар успешно обновлен!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message); // Добавляем ошибку в модель
                }
            }
            return View(product);
        }

        // GET: /Products/Delete/5
        // Отображает страницу с подтверждением удаления.
        public IActionResult Delete(int id)
        {
            var product = repository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST: /Products/Delete/5
        // Окончательное подтверждение удаления.
        [HttpPost, ActionName("Delete")] // ActionName связывает этот метод с формой,
                                         // которая ведет на действие "Delete"
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            TempData["SuccessMessage"] = "Товар удален!";
            return RedirectToAction(nameof(Index));
        }
        // GET: /Products/Category/Электроника
        public IActionResult Category(string category)
        {
            var products = repository.GetByCategory(category);
            ViewBag.Category = category;
            return View(products); // Используем то же представление Index.cshtml для отображения списка
        }
        // GET: /Products/InStock
        public IActionResult InStock()
        {
            var products = repository.GetInStock();
            return View("Index", products); // Явно указываем,
                                            // что нужно использовать представление Index.cshtml
        }

        /*Новые LINQ действия*/
        // GET: /Products/ByPrice?min=100&max=1000
        public IActionResult ByPrice(decimal min, decimal max)
        {
            var products = repository.GetProductsByPriceRange(min, max);
            ViewBag.MinPrice = min;
            ViewBag.MaxPrice = max;
            ViewBag.Title = $"Товары от {min:C} до {max:C}";
            return View(products);
        }
        // GET: /Products/TopExpensive?count=5
        public IActionResult TopExpensive(int count = 5)
        {
            var products = repository.GetTopExpensiveProducts(count);
            ViewBag.Title = $"Топ {count} самых дорогих товаров";
            ViewBag.Count = count;
            return View(products);
        }
        // GET: /Products/Search?term=ноутбук
        public IActionResult Search(string term)
        {
            //if (string.IsNullOrWhiteSpace(term))
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            var products = repository.SearchProducts(term);
            ViewBag.SearchTerm = term;
            ViewBag.Title = $"Результаты поиска: {term}";
            ViewBag.Count = products.Count();
            return View(products);
        }
        // GET: /Products/Statistics
        public IActionResult Statistics()
        {
            // Получаем все товары из репозитория
            var products = repository.GetAll();
            // Вычисляем общую статистику
            var stats = new ProductsStatisticsViewModel
            {
                TotalCount = repository.GetTotalCount(),
                AveragePrice = repository.GetAveragePrice(),
                InStockCount = repository.GetInStock().Count(),
                PriceRange = repository.GetPriceRange(),
                Categories = products.GroupBy(p => p.Category)
                .Select(g => new CategoryStatViewModel
                {
                    Category = g.Key ?? "Без категории",
                    Count = g.Count(),
                    AveragePrice = g.Average(p => p.Price),
                    MinPrice = g.Min(p => p.Price),
                    MaxPrice = g.Max(p => p.Price)
                })
                .OrderBy(c => c.Category)
            };
            return View(stats);
        }
        // GET: /Products/GroupedByCategory
        public IActionResult GroupedByCategory()
        {
            var products = repository.GetAll(); // Все товары
            return View(products);
        }
        // GET: /Products/Paginated?page=1
        public IActionResult Paginated(int page = 1, int pageSize = 5)
        {
            var products = repository.GetProductsWithPagination(page, pageSize);
            var totalPages = repository.GetTotalPages(pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;
            return View(products);
        }
    }
}