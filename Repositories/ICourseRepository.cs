using MvcApp.Models;
using System.Collections.Generic;

namespace MvcApp.Repositories
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAll();
        Course? GetById(int id);
        void Add(Course course);
        void Update(Course course);
        void Delete(int id);
        bool Exists(int id); // для проверки существования перед операциями
    }
}