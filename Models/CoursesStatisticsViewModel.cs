using System.Collections.Generic;

namespace MvcApp.Models
{
    /// <summary>
    /// ViewModel для страницы статистики курсов
    /// </summary>
    public class CoursesStatisticsViewModel
    {
        /// Общее количество курсов
        public int TotalCount { get; set; }
        /// Средняя длительность курсов в днях
        public double AverageDuration { get; set; }
        /// Диапазон часов (минимальное, максимальное)
        public (int MinHours, int MaxHours) HoursRange { get; set; }
        /// Диапазон кредитов (минимальное, максимальное)
        public (int MinCredits, int MaxCredits) CreditsRange { get; set; }
        /// Список групп по кредитам со статистикой
        public IEnumerable<CreditsGroupStatViewModel> CreditsGroups { get; set; }
    }

    /// <summary>
    /// Статистика по группе кредитов
    /// </summary>
    public class CreditsGroupStatViewModel
    {
        /// Количество кредитов
        public int Credits { get; set; }
        /// Количество курсов с таким количеством кредитов
        public int Count { get; set; }
        /// Средняя длительность курсов в этой группе (дни)
        public double AverageDuration { get; set; }
        /// Среднее количество часов в этой группе
        public double AverageHours { get; set; }
        /// Минимальная длительность в группе
        public int MinDuration { get; set; }
        /// Максимальная длительность в группе
        public int MaxDuration { get; set; }
    }
}