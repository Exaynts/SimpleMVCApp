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

        // GET: Courses/Details/5
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
        [ValidateAntiForgeryToken] // Для проверки и наличия уникального токена при отправке запроса для защиты от межсайтовой подделки запроса
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
    }
}