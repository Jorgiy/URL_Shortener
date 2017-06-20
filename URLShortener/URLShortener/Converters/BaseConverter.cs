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
        public static string ConvertFrom10To36(int input)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";

            string result = "";

            while (input > 0)
            {
                result += chars[input % 36];
                input /= 36;
            }

            return result;
        }
    }
}