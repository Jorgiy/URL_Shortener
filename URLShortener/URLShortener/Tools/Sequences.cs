using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace URLShortener.Tools
{
    /// <summary>
    /// класс для получения уникальных айди из последовательностей СУБД
    /// </summary>
    public static class Sequences
    {
        /// <summary>
        /// метод для получения уникального айди для токенов пользователей
        /// </summary>
        /// <returns>уникальный айди</returns>
        public static int GetNewId(SequencesTypes type)
        {
            using (
                    var connection =
                        new SqlConnection(
                            ConfigurationManager.ConnectionStrings["UrlShortenerBaseEntities"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand($"SELECT NEXT VALUE FOR [dbo].[{type}]"))
                {
                    command.Connection = connection;
                    var reader = command.ExecuteReader();
                    return reader.GetInt32(0);
                }
            }
        }

        /// <summary>
        /// типы возможных последовательностей
        /// </summary>
        public enum SequencesTypes
        {
            /// <summary>
            /// выдать айди для токена
            /// </summary>
            NextUserId,
            /// <summary>
            /// выдать айди для ссылки
            /// </summary>
            NextUrlId
        }
    }
}