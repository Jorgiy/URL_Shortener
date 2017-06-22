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
        public static void Log(ErrorType type, string text)
        {
            Task.Run(() =>
            {
                lock (LockObject)
                {
                    var datetime = DateTime.UtcNow;
                    try
                    {
                        using (
                            var file = new FileStream($"D:/Logs/{datetime.Date:yyyy-MM-dd}_{type}.txt",
                                FileMode.Append, FileAccess.Write))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.Write(
                                    $"{datetime.Hour}:{datetime.Minute}:{datetime.Second} {text ?? String.Empty}\n");
                            }
                        }
                    }
                    catch (Exception exc)
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
    }

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