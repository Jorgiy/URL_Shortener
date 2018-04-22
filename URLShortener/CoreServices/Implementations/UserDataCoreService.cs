using System.Collections.Generic;
using System.Linq;
using AspNet.Mvc.Grid.Sorting;
using CoreServices.Interfaces;
using CoreServices.Models;
using DataLayer.Interfaces;

namespace CoreServices.Implementations
{
    public class UserDataCoreService : IUserDataCoreService
    {
        private readonly ITokenMappingRepository _tokenMappingRepository;

        public UserDataCoreService(ITokenMappingRepository tokenMappingRepository)
        {
            _tokenMappingRepository = tokenMappingRepository;
        }
        
        public IEnumerable<DisplayedLinkResult> GetUserPaginatedLinks(string token, SortDirection direction, int sortColumn)
        {
            return _tokenMappingRepository.GetFullLinkInformationByToken(token, direction, sortColumn).Select(page =>
                new DisplayedLinkResult
                {
                    CreationDate = page.CreationDateTime,
                    Follows = page.Link.Follows,
                    OriginalLink = page.Link.Url,
                    ShortedLink = page.Link.ShortUrl
                });
        }
    }
}