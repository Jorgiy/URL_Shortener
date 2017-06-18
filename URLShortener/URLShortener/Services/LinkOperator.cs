using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using URLShortener.Converters;
using URLShortener.DataContexts;
using URLShortener.Interfaces;
using URLShortener.Tools;
using static URLShortener.Tools.Sequences.SequencesTypes;

namespace URLShortener.Services
{
    public class LinkOperator : ILinkOperator
    {
        /// <summary>
        /// объект для блокировки
        /// </summary>
        private static readonly object Lock = new object();

        public ILinkCreationResult CreateLink(string url)
        {
            var regex =
                new Regex(
                    @"\(?(?:(http|https|ftp):\/\/)?(?:((?:[^\W\s]|\.|-|[:]{1})+)@{1})?((?:www.)?(?:[^\W\s]|\.|-)+[\.][^\W\s]{2,4}|localhost(?=\/)|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(?::(\d*))?([\/]?[^\s\?]*[\/]{1})*(?:\/?([^\s\n\?\[\]\{\}\#]*(?:(?=\.)){1}|[^\s\n\?\[\]\{\}\.\#]*)?([\.]{1}[^\s\?\#]*)?)?(?:\?{1}([^\s\n\#\[\]]*))?([\#][^\s\n]*)?\)?");

            if (url.Length > 850)
                return new LinkCreationResult()
                {
                    Success = false,
                    ErrorMessage = "Слишком длинная ссылка, попробуйте уложиться в 850 символов"
                };

            if (regex.Match(url).Value != url)
                return new LinkCreationResult() {Success = false, ErrorMessage = "Введите правильную ссылку"};

            var db = new UrlShortenerBaseEntities();

            lock (Lock)
            {
                var possibleShortUrl = db.Links.FirstOrDefault(c => c.Url == url.Replace(@"http://", String.Empty));
                    // c http:// в базу не пишем
                if (possibleShortUrl != null)
                    return new LinkCreationResult()
                    {
                        LinkId = possibleShortUrl.Id,
                        ShortLink = possibleShortUrl.ShortUrl,
                        Success = true
                    };

                try
                {
                    var nextUrlId = Tools.Sequences.GetNewId(NextUrlId);
                    
                    if (nextUrlId <= 0)
                        throw new BuisenessException(
                            $"Последовательность выдала id для новой ссылки = {nextUrlId} при попытке создать ссылку.",
                            null)
                        {
                            ErrorLevel = ErrorType.Critical
                        };

                    var shortUrl = BaseConverter.ConvertFrom10To36(CongruentMethod.MakeConversion(nextUrlId));

                    db.Links.Add(new Links()
                    {
                        CreationDate = DateTime.Now,
                        Follows = 0,
                        Id = nextUrlId,
                        ShortUrl = shortUrl,
                        Url = url.Replace(@"http://", String.Empty)
                    });

                    db.SaveChanges();

                    return new LinkCreationResult() {LinkId = nextUrlId, ShortLink = shortUrl, Success = true};
                }
                catch (Exception exc)
                {
                    throw new BuisenessException("Ошибка на стороне базы данных при попытке создать ссылку.", exc)
                    {
                        ErrorLevel = ErrorType.Critical
                    };
                }
            }
        }

        public string ReturnOriginalLink(string shortenLink)
        {
            var db = new UrlShortenerBaseEntities();
            return db.Links.FirstOrDefault(c => c.ShortUrl == shortenLink)?.Url;
        }
    }

    public class LinkCreationResult : ILinkCreationResult
    {
        public string ShortLink { get; set; }
        public int LinkId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}