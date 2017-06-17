using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URLShortener.Tools
{
    /// <summary>
    /// Класс для определения уровня ошибок
    /// </summary>
    public class BuisenessException : Exception
    {
        public BuisenessException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Уровень ошибки
        /// </summary>
        public ErrorType ErrorLevel { get; set; }
    }
}