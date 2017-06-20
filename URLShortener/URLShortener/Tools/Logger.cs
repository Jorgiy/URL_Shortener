﻿using System;
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
        public static void Log(ErrorType type, string text, DateTime errorDateTime)
        {
            Task.Run(() =>
            {
                lock (LockObject)
                {
                    try
                    {
                        using (
                            var file = new FileStream($"D:/Logs/{errorDateTime.Date:yyyy-MM-dd}_{type}.txt",
                                FileMode.Append, FileAccess.Write))
                        {
                            using (var writer = new StreamWriter(file))
                            {
                                writer.Write(
                                    $"{errorDateTime.Hour}:{errorDateTime.Minute}:{errorDateTime.Second} {text ?? String.Empty}\n");
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