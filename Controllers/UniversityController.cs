using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class UniversityController : Controller
    {
        // Кафедры факультета информационных технологий
        private static List<Department> digitTechnologies = new List<Department>(3) {
            new Department(1, "Доцент, к.т.н. Елена Николаевна Новикова",
                "Кафедра программной инженерии", "Разработка ПО, мобильная разработка, веб-технологии", 
                15, "software@it.university.ru", "9 корпус, 413 - 529", "https://i.ibb.co/C30prpRB/code.avif"){
            },
            new Department(1, "Профессор, д.ф.-м.н. Дмитрий Константинович Волков",
                "Кафедра информационной безопасности", "Криптография, защита данных, киберразведка",
                10, "security@it.university.ru", "22 корпус, 413 - 529", "https://i.ibb.co/1tcTkkg2/information-security.jpg"){
            },
            new Department(1, "Доцент, к.т.н. Мария Андреевна Кошкина",
                "Кафедра искусственного интеллекта", "Разработка ПО, мобильная разработка, веб-технологии",
                12, "ai@it.university.ru", "9 корпус, 215 - 220", "https://i.ibb.co/zWvBLgvW/ai.png"){
            },
        };

        // Кафедры биологического факультета
        private static List<Department> biology = new List<Department>() {
            new Department(2, "Профессор, д.б.н. Дмитрий Александрович Белозерский",
                "Кафедра биохимии и молекулярной биологии", "Молекулярная генетика, ферментология, " +
                "протеомика, биоинформатика", 15, "biochem@bio.university.ru", "Биолого-почвенный корпус, 401-410",
                "https://i.ibb.co/GvW75Yv6/genetics.jpg"){
            },
            new Department(2, "Доцент, к.б.н. Владимир Петрович Орлов",
                "Кафедра зоологии и экологии", "Зоология позвоночных и беспозвоночных, экология сообществ, охрана природы",
                11, "zoology@bio.university.ru", "Биолого-почвенный корпус, 201-210", "https://i.ibb.co/5hj9nTWD/zoology.webp"){
            },
            new Department(2, "Профессор, д.б.н. Татьяна Михайловна Цветкова",
                "Кафедра ботаники и физиологии растений", "Систематика растений, " +
                "физиология растений, фитоценология, бриология", 10, "botany@bio.university.ru",
                "Биолого-почвенный корпус, 301-308", "https://i.ibb.co/QFp3rk1F/botany.jpg"){
            },
            new Department(2, "Доцент, к.б.н. Алексей Викторович Тихомиров",
                "Кафедра микробиологии и вирусологии", "Микробиология, " +
                "вирусология, микробиотехнология", 16, "microbio@bio.university.ru",
                "Биолого-почвенный корпус, 501-506", "https://i.ibb.co/B59hX2Q1/microbio.webp"){
            }
        };

        private List<Faculty> faculties = new() {
            new Faculty(1, "Факультет информационных технологий", "Digit Technologies", "it@university.ru",
                "Ведущий IT-факультет, готовящий специалистов в области " +
                "программирования, искусственного интеллекта и " +
                "кибербезопасности", "Профессор, д.т.н. Александр Игоревич Смирнов",
                "https://i.ibb.co/yBFDjtFJ/digital-technologies.jpg", "Математика, Русский язык, Информатика / Физика,", 
                50, 250000, digitTechnologies),

            new Faculty(2, "Биологический факультет", "Biology", "bio@university.ru",
                "Изучение жизни во всех её проявлениях — от молекулярных механизмов " +
                "до экосистем планеты. Современные лаборатории, " +
                "полевые практики и участие в реальных научных проектах", "Профессор, д.б.н. Надежда Викторовна Левина",
                "https://i.ibb.co/FLD6fW70/biology.jpg", "Математика, Русский язык, Биология / Химия,",
                60, 240000, biology)
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Title";
            ViewBag.Faculties = faculties;
            ViewBag.Time = DateTime.Now.ToString("dd.MM.yyyy");
            return View();
        }

        public IActionResult Faculties()
        {
            ViewBag.Faculties = faculties;
            return View();
        }

        [Route("University/Faculty/{facultyName}")]
        public IActionResult Faculty(string facultyName)
        {
            Faculty faculty = new Faculty();          
            // Ищем факультет с нужным именем
            for (int i = 0; i < faculties.Count; ++i)
            {
                if(faculties[i].Name.ToLower() == facultyName.ToLower() ||
                    faculties[i].DubbedName.ToLower() == facultyName.ToLower())
                {
                    faculty = faculties[i];
                    break;
                }
            }

            // Если факультет не был найден
            if(faculty.Name == string.Empty)
            {
                return Content($"Факультета с названием {facultyName} не существует!");
            }

            ViewBag.Faculty = faculty;
            ViewBag.Departments = faculty.Departments;
            return View();
        }

        [Route("University/Departments/{facultyId}")]
        public IActionResult Departments(int facultyId)
        {
            // Проверяем на выход за пределы списка с факультетами
            if (facultyId < 1 || facultyId > faculties.Count)
            {
                return Content($"Факультет с id {facultyId} не найден.");
            }               
            else
            {
                var faculty = faculties[facultyId - 1];
                ViewBag.Faculty = faculty;
                ViewBag.Departments = faculty.Departments;
            }             
            return View();
        }
    }
}
