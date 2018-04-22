using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AspNet.Mvc.Grid.Sorting;
using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class TokenMappingRepository : ITokenMappingRepository
    {
        private readonly UrlShortenerContext _dbContext;
        
        public TokenMappingRepository(UrlShortenerContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public TokenMapping InsertNewMapping(TokenMapping insertEntity)
        {
            var inserted = _dbContext.TokenMappings.Add(insertEntity);
            _dbContext.SaveChanges();
            return inserted;
        }

        public bool HasUserLink(int tokenId, int linkId)
        {
            return _dbContext.TokenMappings.Any(x => x.TokenId == tokenId && x.LinkId == linkId);
        }

        public IEnumerable<TokenMapping> GetFullLinkInformationByToken(string token, SortDirection sortDirection, int sortColumn)
        {
            if (token == null) return new List<TokenMapping>();
            
            var sortCol = Enum.IsDefined(typeof(SortCpoumnTypes), sortColumn)
                ? (SortCpoumnTypes)sortColumn
                : SortCpoumnTypes.CreationDate;


            return SortColumnChoise(sortCol,
                _dbContext.TokenMappings.Include(x => x.Token)
                    .Include(x => x.Link)
                    .Where(c => c.Token.TokenString == token)
                , sortDirection
            );
        }
        
        private IQueryable<TokenMapping> SortColumnChoise(SortCpoumnTypes column, IQueryable<TokenMapping> input,
            SortDirection direction)
        {
            switch (column)
            {
                case SortCpoumnTypes.OriginalLink:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Link.Url)
                        : input.OrderByDescending(c => c.Link.Url);
                case SortCpoumnTypes.ShortedLink:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Link.ShortUrl)
                        : input.OrderByDescending(c => c.Link.ShortUrl);
                case SortCpoumnTypes.CreationDate:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.CreationDateTime)
                        : input.OrderByDescending(c => c.CreationDateTime);
                case SortCpoumnTypes.Follows:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.Link.Follows)
                        : input.OrderByDescending(c => c.Link.Follows);
                default:
                    return direction == SortDirection.Ascending
                        ? input.OrderBy(c => c.CreationDateTime)
                        : input.OrderByDescending(c => c.CreationDateTime);
            }
        }
    }
}