using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using URLShortener.DataContexts;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.Tools;

namespace URLShortener.Services
{
    public class UserDataDisplay : IUserDataDisplay
    {
        public List<IDisplayedLink> GetUserPaginatedLinks(string token, int pageSize, SortDirection direction, int sortColumn, int pageNumber)
        {
            if (token == null) return new List<IDisplayedLink>();
            
            var db = new UrlShortenerBaseEntities();
            var sortCol = Enum.IsDefined(typeof(SortCpoumnTypes), sortColumn)
                ? (SortCpoumnTypes) sortColumn
                : SortCpoumnTypes.CreatioDate;

            try
            {
                if (direction == SortDirection.Ascending)
                {
                    return
                        new List<IDisplayedLink>(db.TokenMapping.Include("Tokens")
                            .Include("Links")
                            .Where(c => c.Tokens.Token == token)
                            .OrderBy(fields => SortColumnChoise(sortCol, fields))
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(page => new DisplayedLink()
                            {
                                CreationDate = page.Links.CreationDate,
                                Follows = page.Links.Follows,
                                OriginalLink = page.Links.Url,
                                ShortedLink = page.Links.ShortUrl
                            }).ToList());
                }
                else
                {
                    return
                        new List<IDisplayedLink>(db.TokenMapping.Include("Tokens")
                            .Include("Links")
                            .Where(c => c.Tokens.Token == token)
                            .OrderByDescending(fields => SortColumnChoise(sortCol, fields))
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(page => new DisplayedLink()
                            {
                                CreationDate = page.Links.CreationDate,
                                Follows = page.Links.Follows,
                                OriginalLink = page.Links.Url,
                                ShortedLink = page.Links.ShortUrl
                            }).ToList());
                }
            }
            catch (Exception exc)
            {
                throw new BuisenessException(
                    "Ошибка на стороне базы данных при попытке отображения ссылок пользователя.", exc)
                {
                    ErrorLevel = ErrorType.Critical
                };
            }
        }

        /// <summary>
        /// Метод для определения колонки сортировки
        /// </summary>
        /// <param name="column">колонка</param>
        /// <param name="input">входная сущность</param>
        /// <returns></returns>
        private static object SortColumnChoise(SortCpoumnTypes column, TokenMapping input)
        {
            switch (column)
            {
                case SortCpoumnTypes.OriginalLink:
                    return input.Links.Url;
                case SortCpoumnTypes.ShortedLink:
                    return input.Links.ShortUrl;
                case SortCpoumnTypes.CreatioDate:
                    return input.Links.CreationDate;
                case SortCpoumnTypes.Follows:
                    return input.Links.Follows;
                default:
                    return input.Links.CreationDate;
            }
        }

        /// <summary>
        /// Колонки для сортировки
        /// </summary>
        public enum SortCpoumnTypes : int
        {
            /// <summary>
            /// Оригинальная ссылка
            /// </summary>
            OriginalLink,
            /// <summary>
            /// укороченная ссылка
            /// </summary>
            ShortedLink,
            /// <summary>
            /// дата создания
            /// </summary>
            CreatioDate,
            /// <summary>
            /// количестов переходдов по ссылке
            /// </summary>
            Follows
        }
    }
}