using MvcApp.Models;
using System.Collections.Generic;

namespace MvcApp.Repositories
{
    public interface ICourseRepository
    {
        // Существующие методы
        IEnumerable<Course> GetAll();
        Course? GetById(int id);
        void Add(Course course);
        void Update(Course course);
        void Delete(int id);
        bool Exists(int id);

        // МЕТОДЫ ДЛЯ LINQ-ЗАПРОСОВ
        // Фильтрация по часам (диапазон)
        IEnumerable<Course> GetCoursesByHoursRange(int minHours, int maxHours);
        // Топ N самых длительных курсов (по длительности в днях)
        IEnumerable<Course> GetTopLongestCourses(int count);
        // Поиск по названию, описанию, преподавателю
        IEnumerable<Course> SearchCourses(string searchTerm);
        // Средняя длительность курсов в днях
        double GetAverageDuration();
        // Пагинация
        IEnumerable<Course> GetCoursesWithPagination(int page, int pageSize);
        // Общее количество страниц
        int GetTotalPages(int pageSize);
    }
}