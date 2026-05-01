using System;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название курса обязательно")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Название должно быть от 5 до 200 символов")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание обязательно")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Описание должно содержать не менее 10 символов")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Преподаватель обязателен")]
        public string Instructor { get; set; } = string.Empty;

        [Display(Name = "Оценка")]
        [Range(1, 10, ErrorMessage = "Количество кредитов должно быть от 1 до 10")]
        public int Credits { get; set; }

        [Range(1, 100, ErrorMessage = "Количество часов должно быть от 1 до 100")]
        public int Hours { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        [CustomValidation(typeof(Course), nameof(ValidateEndDate))]
        public DateTime EndDate { get; set; }

        // Вычисляет длительность курса в днях.
        public int GetDuration()
        {
            return (EndDate - StartDate).Days;
        }

        /// Кастомная валидация. Дата окончания курса должна быть позже даты его начала.
        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var course = (Course)context.ObjectInstance;
            if (endDate <= course.StartDate)
            {
                return new ValidationResult("Дата окончания курса должна быть позже даты его начала.");
            }
            return ValidationResult.Success!;
        }
    }
}