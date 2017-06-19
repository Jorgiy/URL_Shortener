using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.Interfaces
{
    /// <summary>
    /// Интерфейс, для реализации конеченого создания ссылок
    /// </summary>
    public interface ILinkOperator
    {
        /// <summary>
        /// Метод укорачивающий ссылку и добавляющий её в БД
        /// </summary>
        /// <param name="url">Ссылка</param>
        /// <returns>результат операции</returns>
        ILinkCreationResult CreateLink(string url);

        /// <summary>
        /// метод, возвращающий оригинальную ссылку
        /// </summary>
        /// <param name="shortenLink">укороченная ссылка</param>
        /// <returns>оригинальная ссылка</returns>
        string ReturnOriginalLink(string shortenLink);

        /// <summary>
        /// метод для подсчёта перехода по ссылкам
        /// </summary>
        /// <param name="shortenLink"></param>
        void IncrementFollowsAsync(string shortenLink);
    }

    /// <summary>
    /// Результат создания ссылки
    /// </summary>
    public interface ILinkCreationResult
    {
        /// <summary>
        /// Новая, зарегистрированная короткая ссылка
        /// </summary>
        string ShortLink { get; set; }
        /// <summary>
        /// Айди ссылки в базе
        /// </summary>
        int LinkId { get; set; }
        /// <summary>
        /// Успешно или нет прошло добавление ссылки
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// Если была ошибка, то тут содержится её текст
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
