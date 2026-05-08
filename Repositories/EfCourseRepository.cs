using Microsoft.EntityFrameworkCore;
using MvcApp.Data;
using MvcApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MvcApp.Repositories
{
    public class EfCourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public EfCourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course? GetById(int id)
        {
            return _context.Courses.Find(id);
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public bool Exists(int id)
        {
            return _context.Courses.Any(c => c.Id == id);
        }

        public IEnumerable<Course> GetCoursesByHoursRange(int minHours, int maxHours)
        {
            return _context.Courses
                .Where(c => c.Hours >= minHours && c.Hours <= maxHours)
                .OrderBy(c => c.Hours)
                .ToList();
        }

        /// <summary>
        /// Получение топ N самых длительных курсов (по длительности в днях)
        /// </summary>
        public IEnumerable<Course> GetTopLongestCourses(int count)
        {
            // Длительность вычисляем как разницу дней в SQL с помощью EF.Functions
            return _context.Courses
                .OrderByDescending(c => EF.Functions.DateDiffDay(c.StartDate, c.EndDate))
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Поиск курсов по названию, описанию или преподавателю
        /// </summary>
        public IEnumerable<Course> SearchCourses(string searchTerm)
        {
            return _context.Courses
                .Where(c => c.Title.Contains(searchTerm) ||
                            c.Description.Contains(searchTerm) ||
                            c.Instructor.Contains(searchTerm))
                .OrderBy(c => c.Title)
                .ToList();
        }

        /// <summary>
        /// Средняя длительность курсов в днях
        /// </summary>
        public double GetAverageDuration()
        {
            return _context.Courses
                .Average(c => EF.Functions.DateDiffDay(c.StartDate, c.EndDate));
        }

        /// <summary>
        /// Пагинация: получение курсов для указанной страницы
        /// </summary>
        public IEnumerable<Course> GetCoursesWithPagination(int page, int pageSize)
        {
            return _context.Courses
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public int GetTotalPages(int pageSize)
        {
            var totalCount = _context.Courses.Count();
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }

}