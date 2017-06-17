using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace URLShortener.Tools
{
    /// <summary>
    /// для логгирования ошибок
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Метод, логгирующий ошибки
        /// </summary>
        /// <param name="type">тип ошибки</param>
        /// <param name="text">содержание</param>
        /// <param name="errorDateTime">время и дата, когда ошибка появилась</param>
        public static async void LogAsync(ErrorType type, string text, DateTime errorDateTime)
        {
            await Task.Run(() =>
            {
                lock (LockObject)
                {
                    try
                    {
                        using (
                            var file = new FileStream($"{errorDateTime.Date:yyyy-MM-dd}_{type.ToString()}.txt",
                                FileMode.Append, FileAccess.ReadWrite))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.Write(
                                    $"{errorDateTime.Hour}:{errorDateTime.Minute}:{errorDateTime.Second} {text}\n");
                            }
                        }
                    }
                    catch
                    {
                        return; // для будущей реализации на случай поломки логгера
                    }
                }
            });
        }

        /// <summary>
        /// объект блокировки
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Тип ошибок
        /// </summary>
        public enum ErrorType
        {
            /// <summary>
            /// Критическая
            /// </summary>
            Critical,
            /// <summary>
            /// Второстепенная, например, не передались cookie
            /// </summary>
            Regular
        }
    }
}