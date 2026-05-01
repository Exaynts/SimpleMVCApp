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
    }
}