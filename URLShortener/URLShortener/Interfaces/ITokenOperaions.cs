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

        /// <summary>
        /// Выдаёт токен пользователю в cookies
        /// </summary>
        /// <param name="token">сам токен</param>
        /// <param name="tokenId">айди токена в базе, для его удаления в случае сетевой ошибки с cookies</param>
        /// <returns>успешно или не успешно отправлен токен пользователю</returns>
        bool AssignTokenToUser(Guid token, int tokenId);
    }

    /// <summary>
    /// Интерфейс, представляющий собой резуьтат создания cookie-токена
    /// </summary>
    public interface ITokenCreationResult
    {
        /// <summary>
        /// Выданный Cookie
        /// </summary>
        Guid Cookie { get; set; }
        /// <summary>
        /// Айди cookie в базе
        /// </summary>
        int TokenId { get; set; }
        /// <summary>
        /// Результат, говорящий о том, что и базе и у пользователя есть токен
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// Если была ошибка, то тут содержится её текст
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
