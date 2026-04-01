namespace MvcApp.Models
{
    public class Department
    {
        public string Head {  get; set; }   // Заведующий    
        public int FacultyId { get; set; } // Id факультета
        public string Name { get; set; } // Название
        public string Profile { get; set; } // Профильные направления
        public int Professors {  get; set; } // Количество преподавателей
        public string Email { get; set; } // Почта
        public string Location { get; set; } // Аудитории
        public string ImagePath { get; set; } // Путь к изображению

        public Department(int facultyId, string head, string name,
            string profile, int professors, string email, string location,
            string imagePath)
        {
            FacultyId = facultyId;
            Head = head;
            Name = name;
            Profile = profile;
            Professors = professors;
            Email = email;
            Location = location;
            ImagePath = imagePath;
        }

    }
}
