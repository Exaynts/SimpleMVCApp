using System;
using System.Collections.Generic;
using System.Linq;
using MvcApp.Models;

namespace MvcApp.Repositories
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        private readonly List<Course> _courses;
        private int _nextId;

        public InMemoryCourseRepository()
        {
            _courses = new List<Course>();
            _nextId = 1;
            SeedData();
        }

        public IEnumerable<Course> GetAll() => _courses;

        public Course GetById(int id) => _courses.FirstOrDefault(c => c.Id == id);

        public void Add(Course course)
        {
            course.Id = _nextId++;
            _courses.Add(course);
        }

        public void Update(Course course)
        {
            var existing = GetById(course.Id);
            if (existing != null)
            {
                existing.Title = course.Title;
                existing.Description = course.Description;
                existing.Instructor = course.Instructor;
                existing.Credits = course.Credits;
                existing.Hours = course.Hours;
                existing.StartDate = course.StartDate;
                existing.EndDate = course.EndDate;
            }
        }

        public void Delete(int id)
        {
            var course = GetById(id);
            if (course != null)
                _courses.Remove(course);
        }

        public bool Exists(int id) => _courses.Any(c => c.Id == id);

        private void SeedData()
        {
            Add(new Course
            {
                Title = "Введение в ASP.NET Core",
                Description = "Изучение основ создания веб-приложений с использованием ASP.NET Core MVC.",
                Instructor = "Новикова Елена",
                Credits = 5,
                Hours = 40,
                StartDate = new DateTime(2025, 9, 1),
                EndDate = new DateTime(2026, 5, 31)
            });

            Add(new Course
            {
                Title = "Entity Framework Core",
                Description = "Работа с базами данных в .NET через ORM Entity Framework Core.",
                Instructor = "Еленов Николай",
                Credits = 4,
                Hours = 32,
                StartDate = new DateTime(2026, 2, 1),
                EndDate = new DateTime(2026, 4, 30)
            });

            Add(new Course
            {
                Title = "JavaScript для back-end разработчиков",
                Description = "Углублённое изучение JavaScript и его применение на серверной стороне.",
                Instructor = "Николева Новика",
                Credits = 3,
                Hours = 80,
                StartDate = new DateTime(2025, 2, 1),
                EndDate = new DateTime(2026, 5, 31)
            });
        }
    }
}