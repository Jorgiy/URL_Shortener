using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AspNet.Mvc.Grid.Pagination;
using AspNet.Mvc.Grid.Sorting;
using URLShortener.DataContexts;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.Tools;

namespace URLShortener.Services
{
    public class UserDataDisplay : IUserDataDisplay
    {
        public IEnumerable<IDisplayedLink> GetUserPaginatedLinks(string token, int pageSize, SortDirection direction, int sortColumn, int pageNumber)
        {
            if (token == null) return new List<IDisplayedLink>();
            
            var db = new UrlShortenerBaseEntities();
            var sortCol = Enum.IsDefined(typeof(SortCpoumnTypes), sortColumn)
                ? (SortCpoumnTypes)sortColumn
                : SortCpoumnTypes.CreationDate;


            return SortColumnChoise(sortCol,
                    db.TokenMapping.Include("Tokens")
                        .Include("Links")
                        .Where(c => c.Tokens.Token == token)
                    , direction
                ).Select(page => new DisplayedLink()
                {
                    CreationDate = page.Links.CreationDate,
                    Follows = page.Links.Follows,
                    OriginalLink = page.Links.Url,
                    ShortedLink = page.Links.ShortUrl
                });
        }

        /// <summary>
        /// Метод для определения колонки сортировки
        /// </summary>
        /// <param name="column">колонка</param>
        /// <param name="input">входная сущность</param>
        /// <param name="direction">направление сортировки</param>
        /// <returns></returns>
        private static IQueryable<TokenMapping> SortColumnChoise(SortCpoumnTypes column, IQueryable<TokenMapping> input,
            SortDirection direction)
        {
            switch (column)
            {
                case SortCpoumnTypes.OriginalLink:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Links.Url)
                        : input.OrderByDescending(c => c.Links.Url);
                case SortCpoumnTypes.ShortedLink:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Links.ShortUrl)
                        : input.OrderByDescending(c => c.Links.ShortUrl);
                case SortCpoumnTypes.CreationDate:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Links.CreationDate)
                        : input.OrderByDescending(c => c.Links.CreationDate);
                case SortCpoumnTypes.Follows:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Links.Follows)
                        : input.OrderByDescending(c => c.Links.Follows);
                default:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Links.CreationDate)
                        : input.OrderByDescending(c => c.Links.CreationDate);
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
            CreationDate,
            /// <summary>
            /// количестов переходдов по ссылке
            /// </summary>
            Follows
        }
    }
}