using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using URLShortener.DataContexts;
using URLShortener.Interfaces;

namespace URLShortener.Services
{
    public class LinkOperator : ILinkOperator
    {
        public ILinkCreationResult CreateLink(string url)
        {
            var regex = new Regex(@"(\b(https?|ftp|file)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
            if (regex.Match(url).Value != url)
                return new LinkCreationResult() {Success = false, ErrorMessage = "Введите правильную ссылку"};
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