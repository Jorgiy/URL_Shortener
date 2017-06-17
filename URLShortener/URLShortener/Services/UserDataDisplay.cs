using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using URLShortener.DataContexts;
using URLShortener.Interfaces;
using URLShortener.Models;

namespace URLShortener.Services
{
    public class UserDataDisplay : IUserDataDisplay
    {
        public List<IDisplayedLink> GetUserPaginatedLinks(string token, int pageSize, SortDirection direction, int sortColumn, int pageNumber)
        {
            if (token == null) return new List<IDisplayedLink>();
            
            var db = new UrlShortenerBaseEntities();
            var sortCol = (SortCpoumnTypes) sortColumn;

            if (direction == SortDirection.Ascending)
            {
                return
                    new List<IDisplayedLink>(db.TokenMapping.Include("Tokens")
                        .Include("Links")
                        .Where(c => c.Tokens.Token == token && c.Links.Id == c.LinkId)
                        .OrderBy(fields => SortColumnChoise(sortCol, fields))
                        .Select(page => page.Links)
                        .ToList()
                        .Select(
                            links =>
                                new DisplayedLink()
                                {
                                    OriginalLink = links.Url,
                                    CreationDate = links.CreationDate,
                                    Follows = links.Follows,
                                    ShortedLink = links.ShortUrl
                                }).ToList().GetRange((pageNumber - 1) * pageSize, pageSize));
            }
            else
            {
                return
                    new List<IDisplayedLink>(db.TokenMapping.Include("Tokens")
                        .Include("Links")
                        .Where(c => c.Tokens.Token == token && c.Links.Id == c.LinkId)
                        .OrderByDescending(fields => SortColumnChoise(sortCol, fields))
                        .Select(page => page.Links)
                        .ToList()
                        .Select(
                            links =>
                                new DisplayedLink()
                                {
                                    OriginalLink = links.Url,
                                    CreationDate = links.CreationDate,
                                    Follows = links.Follows,
                                    ShortedLink = links.ShortUrl
                                }).ToList().GetRange((pageNumber - 1) * pageSize, pageSize));
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