using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="tokenId">токен пользователя</param>
        /// <param name="pageSize">количество записей на странице</param>
        /// <param name="direction">направление сортировки</param>
        /// <param name="sortColumn">колонка по которой сортировать</param>
        /// <param name="pageNumber">номер страницы</param>
        /// <returns>пагинированные ссылки</returns>
        List<IDisplayedLink> GetUserPaginatedLinks(int? tokenId, int pageSize, bool direction, int sortColumn, int pageNumber);
    }

    /// <summary>
    /// Интерфейс для отображения ссылки пользователя
    /// </summary>
    public interface IDisplayedLink
    {
        /// <summary>
        /// Оригинальная ссылка
        /// </summary>
        string OriginalLink { get; set; }
        /// <summary>
        /// укороченная часть ссылки
        /// </summary>
        string ShortedLink { get; set; }
        /// <summary>
        /// Дата создания ссылки
        /// </summary>
        DateTime CreationDate { get; set; }
        /// <summary>
        /// Количество посещений
        /// </summary>
        long Follows { get; set; }
    }
}
