using System;
using System.Text.RegularExpressions;
using CoreServices.Interfaces;
using CoreServices.Models;
using DataLayer.Interfaces;
using DataLayer.Models;
using NLog;
using Utils;

namespace CoreServices.Implementations
{
    public class LinkCoreService : ILinkCoreService
    {
        private readonly ILinksRepository _linksRepository;

        private readonly Logger _logger;
        
        private readonly Regex _linkRegex = new Regex(
            @"\(?(?:(http|https|ftp):\/\/)?(?:((?:[^\W\s]|\.|-|[:]{1})+)@{1})?((?:www.)?(?:[^\W\s]|\.|-)+[\.][^\W\s]{2,4}|localhost(?=\/)|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(?::(\d*))?([\/]?[^\s\?]*[\/]{1})*(?:\/?([^\s\n\?\[\]\{\}\#]*(?:(?=\.)){1}|[^\s\n\?\[\]\{\}\.\#]*)?([\.]{1}[^\s\?\#]*)?)?(?:\?{1}([^\s\n\#\[\]]*))?([\#][^\s\n]*)?\)?");

        private const int MaxUrlLength = 850;

        private readonly object _lockNewUrlObject = new object();
        
        private readonly object _lockIncrementFollowsObject = new object();
        
        public LinkCoreService(ILinksRepository linksRepository)
        {
            _linksRepository = linksRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public LinkCreationResult CreateLink(string url)
        {
            var regex = _linkRegex;

            if (url.Length > MaxUrlLength)
                return new LinkCreationResult
                {
                    Success = false,
                    ErrorMessage = "Too long link, try to keep in 850 signs"
                };

            if (regex.Match(url).Value != url || url == String.Empty)
                return new LinkCreationResult() {Success = false, ErrorMessage = "Enter valid link"};

            lock (_lockNewUrlObject)
            {
                if (!new Regex("^http://|^ftp://|^https://").IsMatch(url)) url = url.Insert(0, "http://");

                var possibleShortUrl = _linksRepository.GetByUrl(url);
                if (possibleShortUrl != null)
                    return new LinkCreationResult()
                    {
                        LinkId = possibleShortUrl.Id,
                        ShortLink = possibleShortUrl.ShortUrl,
                        Success = true
                    };

                try
                {          
                    var created = _linksRepository.InsertLink(new Link
                    {
                        Url = url
                    }, new ConversionProvider());

                    return new LinkCreationResult {LinkId = created.Id, ShortLink = created.ShortUrl, Success = true};
                }
                catch (Exception exc)
                {
                    throw new BuisenessException("Error occured while link creation", exc);
                }
            }
        }

        public string ReturnOriginalLink(string shortenLink)
        {
            return _linksRepository.GetByShortUrl(shortenLink).Url;
        }

        public void IncrementFollows(string shortenLink)
        {
            try
            {
                lock (_lockIncrementFollowsObject)
                {
                    _linksRepository.IncrementFollows(shortenLink);
                }
                
            }
            catch (Exception e)
            {
                _logger.Error($"Error occured while incrementing link's ({shortenLink}) follows: {e.Message}");
            }
        }
    }
}