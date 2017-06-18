using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URLShortener.Models
{
    /// <summary>
    /// модель представление результата создания ссылки
    /// </summary>
    public class CreationLinkResultModel
    {
        /// <summary>
        /// успешно ли создалась ссылка
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Исходная ссылка
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Укороченная часть ссылки
        /// </summary>
        public string ShortUrl { get; set; }

        /// <summary>
        /// токен для передачи в cookies
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// создан ли токен
        /// </summary>
        public bool TokenCreated { get; set; }
    }
}