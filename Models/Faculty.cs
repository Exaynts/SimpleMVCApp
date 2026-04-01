namespace MvcApp.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; } // Название

        public string DubbedName { get; set; } // Английская версия названия
        public string Email { get; set; } // Почта
        public string Description { get; set; } // Описание
        public string Dean { get; set; } // Заведующий
        public string LogoPath { get; set; } //  к логотипу
        public string Exams {  get; set; } // Вступительные экзамены
        public int BudgetPlaces { get; set; } // Количество бюджетных мест
        public List<Department> Departments { get; set; } // Список кафедр
        public int Price { get; set; } // Цена за платное обучение (в год)

        public Faculty(int id, string name, string dubbedName, string email, 
            string description, string dean, string logopath,
            string exams, int budgetPlaces, int price,
            List<Department> departments) { 

            Id = id;
            Name = name;
            DubbedName = dubbedName;
            Email = email;
            Description = description;
            Dean = dean;
            LogoPath = logopath;
            Exams = exams;
            BudgetPlaces = budgetPlaces;
            Price = price;
            Departments = departments;
        }

        public Faculty() {
            Departments = new List<Department>();
            Name = string.Empty;
            DubbedName = string.Empty;
            Email = string.Empty;
            Description = string.Empty;
            Dean = string.Empty;
            LogoPath = string.Empty;
            Exams = string.Empty;
            BudgetPlaces = 0;
            Price = 0;
            Id = 0;
        }
    }
}
