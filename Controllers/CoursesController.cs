using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using MvcApp.Repositories;
using System.Linq;

namespace MvcApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _repository;

        public CoursesController(ICourseRepository repository)
        {
            _repository = repository;
        }

        // GET: Courses
        public IActionResult Index()
        {
            var courses = _repository.GetAll();
            return View(courses);
        }

        // GET: Courses/Details/1
        public IActionResult Details(int id)
        {
            var course = _repository.GetById(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int id)
        {
            var course = _repository.GetById(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course course)
        {
            if (id != course.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(int id)
        {
            var course = _repository.GetById(id);
            if (course == null)
                return NotFound();
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // ========== LINQ-ДЕЙСТВИЯ ==========

        // GET: Courses/ByHours?min=10&max=50
        public IActionResult ByHours(int min, int max)
        {
            int maxPossibleHours = 99999; // max Possible number hours of Course
            // Валидация по количеству часов. 0 <= min <= max << maxPossible
            if (min < 0)
            {
                min = 0;
                ModelState.AddModelError("", "Минимальное количество часов должно быть не отрицательным.");
                ViewBag.MinHours = min;
                ViewBag.MaxHours = max;
                // Возвращаем пустой список или последний корректный результат
                var emptyCourses = new List<Course>();
                return View(emptyCourses);
            }
            if (min > max)
            {
                max = min;
                ModelState.AddModelError("", "Минимальное количество часов не может превышать максимальное.");
                ViewBag.MinHours = min;
                ViewBag.MaxHours = max;
                // Возвращаем пустой список или последний корректный результат
                var emptyCourses = new List<Course>();
                return View(emptyCourses);
                }
            if (max > maxPossibleHours)
            {
                max = maxPossibleHours;
                ModelState.AddModelError("", $"Максимальное количество часов должно быть не более {maxPossibleHours}.");
                ViewBag.MinHours = min;
                ViewBag.MaxHours = max;
                // Возвращаем пустой список или последний корректный результат
                var emptyCourses = new List<Course>();
                return View(emptyCourses);
            }
            if (min > maxPossibleHours) min = maxPossibleHours;


            var courses = _repository.GetCoursesByHoursRange(min, max);
            ViewBag.MinHours = min;
            ViewBag.MaxHours = max;
            ViewBag.Title = $"Курсы от {min} до {max} часов";
            return View(courses);
        }

        // GET: Courses/TopLongest?count=3
        public IActionResult TopLongest(int count = 3)
        {
            var courses = _repository.GetTopLongestCourses(count);
            ViewBag.Title = $"Топ {count} самых длительных курсов (по дням)";
            ViewBag.Count = count;
            return View(courses);
        }

        // GET: Courses/Search?term=ASP
        public IActionResult Search(string term)
        {
            IEnumerable<Course> courses;
            if (string.IsNullOrWhiteSpace(term))
            {
                courses = _repository.GetAll();
                ViewBag.SearchTerm = "";
                ViewBag.Title = "Результаты поиска (все курсы)";
            }
            else
            {
                courses = _repository.SearchCourses(term);
                ViewBag.SearchTerm = term;
                ViewBag.Title = $"Результаты поиска: {term}";
            }
            ViewBag.Count = courses.Count();
            return View(courses);
        }

        // GET: Courses/Statistics
        public IActionResult Statistics()
        {
            var courses = _repository.GetAll();
            var hoursMin = courses.Any() ? courses.Min(c => c.Hours) : 0;
            var hoursMax = courses.Any() ? courses.Max(c => c.Hours) : 0;
            var creditsMin = courses.Any() ? courses.Min(c => c.Credits) : 0;
            var creditsMax = courses.Any() ? courses.Max(c => c.Credits) : 0;

            var stats = new CoursesStatisticsViewModel
            {
                TotalCount = _repository.GetAll().Count(),
                AverageDuration = _repository.GetAverageDuration(),
                HoursRange = (hoursMin, hoursMax),
                CreditsRange = (creditsMin, creditsMax),
                CreditsGroups = courses
                    .GroupBy(c => c.Credits)
                    .Select(g => new CreditsGroupStatViewModel
                    {
                        Credits = g.Key,
                        Count = g.Count(),
                        AverageDuration = g.Average(c => (c.EndDate - c.StartDate).Days),
                        AverageHours = g.Average(c => c.Hours),
                        MinDuration = g.Min(c => (c.EndDate - c.StartDate).Days),
                        MaxDuration = g.Max(c => (c.EndDate - c.StartDate).Days)
                    })
                    .OrderBy(g => g.Credits)
            };
            return View(stats);
        }

        // GET: Courses/GroupedByCredits
        public IActionResult GroupedByCredits()
        {
            var courses = _repository.GetAll();
            return View(courses);
        }

        // GET: Courses/Paginated?page=1&pageSize=3
        public IActionResult Paginated(int page = 1, int pageSize = 3)
        {
            var courses = _repository.GetCoursesWithPagination(page, pageSize);
            var totalPages = _repository.GetTotalPages(pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;
            return View(courses);
        }

        // МЕТОДЫ-ОБЁРТКИ ДЛЯ УНИФИКАЦИИ МЕНЮ LINQ
        // GET: Courses/ByPrice?min=10&max=50  -> перенаправляет на ByHours
        public IActionResult ByPrice(int min, int max)
        {
            return ByHours(min, max);
        }

        // GET: Courses/TopExpensive?count=3 -> перенаправляет на TopLongest
        public IActionResult TopExpensive(int count = 3)
        {
            return TopLongest(count);
        }

        // GET: Courses/GroupedByCategory -> перенаправляет на GroupedByCredits
        public IActionResult GroupedByCategory()
        {
            return GroupedByCredits();
        }
    }
}