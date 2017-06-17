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
        private const uint CoefficientA = 16807;

        /// <summary>
        /// Кэффициент M
        /// </summary>
        private const uint CoefficientM = uint.MaxValue / 2;

        /// <summary>
        /// Кэффициент C
        /// </summary>
        private const uint CoefficientC = 0;

        /// <summary>
        /// Выполняет преобразование, гарантируя отсутствие повторов от 1 до 2^31-1
        /// </summary>
        /// <param name="input">входное число</param>
        /// <returns>псевдослучайный результат</returns>
        public static uint MakeConversion(uint input)
        {
            return (input * CoefficientA + CoefficientC) % CoefficientM;
        }
    }
}