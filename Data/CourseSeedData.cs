using MvcApp.Models;
using System;
using System.Linq;

namespace MvcApp.Data
{
    public static class CourseSeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Courses.Any())
                return;

            var courses = new Course[]
            {
                new Course
                {
                    Title = "Введение в ASP.NET Core",
                    Description = "Изучение основ создания веб-приложений с использованием ASP.NET Core MVC.",
                    Instructor = "Новикова Елена",
                    Credits = 5,
                    Hours = 40,
                    StartDate = new DateTime(2025, 9, 1),
                    EndDate = new DateTime(2026, 5, 31)
                },
                new Course
                {
                    Title = "Entity Framework Core",
                    Description = "Работа с базами данных в .NET через ORM Entity Framework Core.",
                    Instructor = "Еленов Николай",
                    Credits = 4,
                    Hours = 32,
                    StartDate = new DateTime(2026, 2, 1),
                    EndDate = new DateTime(2027, 4, 30)
                },
                new Course
                {
                    Title = "JavaScript для back-end разработчиков",
                    Description = "Углублённое изучение JavaScript и его применение на серверной стороне.",
                    Instructor = "Николева Новика",
                    Credits = 3,
                    Hours = 80,
                    StartDate = new DateTime(2025, 2, 1),
                    EndDate = new DateTime(2026, 5, 31)
                }
            };

            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
    }
}