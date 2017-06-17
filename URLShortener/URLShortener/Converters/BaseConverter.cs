using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URLShortener.Converters
{
    /// <summary>
    /// Для конвертирования из псевдослучайных чисел в строку
    /// </summary>
    public static class BaseConverter
    {
        /// <summary>
        /// Преобразовывает число в короткую строку base36
        /// </summary>
        /// <param name="input">число для преобразования</param>
        /// <returns>результат конвертации</returns>
        public static string ConvertFrom10To36(uint input)
        {
            return Convert.ToString(input, 36);
        }
    }
}