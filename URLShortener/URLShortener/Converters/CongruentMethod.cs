using System;

namespace URLShortener.Converters
{
    /// <summary>
    /// Выполняет прямые преобразования конгруэнтным методом с а = 16807, с = 0 и m = 2^31-1 (как в с++11)
    /// </summary>
    public static class CongruentMethod
    {
        /// <summary>
        /// Кэффициент а
        /// </summary>
        private const int CoefficientA = 16807;

        /// <summary>
        /// Кэффициент M
        /// </summary>
        private const int CoefficientM = int.MaxValue;

        /// <summary>
        /// Кэффициент C
        /// </summary>
        private const int CoefficientC = 0;

        /// <summary>
        /// Выполняет преобразование, гарантируя отсутствие повторов в диапазонах длиной 2^31-1 чисел
        /// </summary>
        /// <param name="input">входное число</param>
        /// <returns>псевдослучайный результат</returns>
        public static int MakeConversion(int input)
        {
            return (input * CoefficientA + CoefficientC) % CoefficientM;
        }
    }
}