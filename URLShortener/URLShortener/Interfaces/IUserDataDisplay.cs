using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Mvc.Grid.Sorting;

namespace URLShortener.Interfaces
{
    /// <summary>
    /// Интерфейс для отображения данных идентифицированному пользователю
    /// </summary>
    public interface IUserDataDisplay
    {
        /// <summary>
        /// Возвращает пагинированные данные 
        /// </summary>
        /// <param name="token">токен пользователя</param>
        /// <param name="pageSize">количество записей на странице</param>
        /// <param name="direction">направление сортировки</param>
        /// <param name="sortColumn">колонка по которой сортировать</param>
        /// <param name="pageNumber">номер страницы</param>
        /// <returns>пагинированные ссылки</returns>
        IPaginatedLinksResult GetUserPaginatedLinks(string token, int pageSize, SortDirection direction, int sortColumn, int pageNumber);
    }

    /// <summary>
    /// интерфейс для отображения пагинированных ссылок пользователя
    /// </summary>
    public interface IPaginatedLinksResult
    {
        /// <summary>
        /// пагинированные ссылки
        /// </summary>
        List<IDisplayedLink> Links { get; set; }
        /// <summary>
        /// общее количество ссылок
        /// </summary>
        int Count { get; set; }
    }

    /// <summary>
    /// Интерфейс для отображения ссылки пользователя
    /// </summary>
    public interface IDisplayedLink
    {
        /// <summary>
        /// Оригинальная ссылка
        /// </summary>
        [Display(Name = "Оригинальная ссылка")]
        string OriginalLink { get; set; }
        /// <summary>
        /// укороченная часть ссылки
        /// </summary>
        [Display(Name = "Укороченная ссылка")]
        string ShortedLink { get; set; }
        /// <summary>
        /// Дата создания ссылки
        /// </summary>
        [Display(Name = "Дата создания")]
        DateTime CreationDate { get; set; }
        /// <summary>
        /// Количество посещений
        /// </summary>
        [Display(Name = "Количество переходов")]
        long Follows { get; set; }
    }
}
