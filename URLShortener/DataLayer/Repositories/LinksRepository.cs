using System.Linq;
using System.Transactions;
using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Models;
using Utils;

namespace DataLayer.Repositories
{
    public class LinksRepository : ILinksRepository
    {
        private readonly UrlShortenerContext _dbContext;
        
        public LinksRepository(UrlShortenerContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Link GetByUrl(string url)
        {
            return _dbContext.Links.SingleOrDefault(x => x.Url == url);
        }

        public Link InsertLink(Link insertEntity, ConversionProvider conversionProvider)
        {
            Link insertedLink = null;
            
            using (var transactionScope = new TransactionScope())
            {
                var addedLink = _dbContext.Links.Add(insertEntity);
                _dbContext.SaveChanges();

                var addedLinkForUpdateShortUrl = _dbContext.Links.Single(x => x.Id == addedLink.Id);
                addedLinkForUpdateShortUrl.ShortUrl =
                    conversionProvider.ConvertIdToShortenString(addedLinkForUpdateShortUrl.Id);
                _dbContext.SaveChanges();
                
                transactionScope.Complete();
                insertedLink = addedLinkForUpdateShortUrl;
            }

            return insertedLink;
        }

        public Link GetByShortUrl(string shortUrl)
        {
            return _dbContext.Links.SingleOrDefault(x => x.ShortUrl == shortUrl);
        }

        public void IncrementFollows(string shortUrl)
        {
            var linkForIncrementFollows = _dbContext.Links.Single(x => x.ShortUrl == shortUrl);
            ++linkForIncrementFollows.Follows;
            _dbContext.SaveChanges();
        }
    }
}