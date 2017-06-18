using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с cookies
    /// </summary>
    public interface ITokenOperaions
    {
        /// <summary>
        /// Метод создающий токен и его связку с ссылкой
        /// </summary>
        /// <param name="linkId">ссылка для привязки токена</param>
        /// <returns>результат создания связки токена в хранилище данных</returns>
        ITokenCreationResult CreateToken(int linkId);
    }

    /// <summary>
    /// Интерфейс, представляющий собой резуьтат создания cookie-токена
    /// </summary>
    public interface ITokenCreationResult
    {
        /// <summary>
        /// Выданный Cookie
        /// </summary>
        string Cookie { get; set; }
        /// <summary>
        /// Айди cookie в базе
        /// </summary>
        int TokenId { get; set; }
        /// <summary>
        /// Результат, говорящий о том, что в базе токен связан с ссылкой
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// Если была ошибка, то тут содержится её текст
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
