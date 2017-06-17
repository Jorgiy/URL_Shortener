using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URLShortener.Interfaces;

namespace URLShortener.Services
{
    public class UserDataDisplay : IUserDataDisplay
    {
        public List<IDisplayedLink> GetUserPaginatedLinks(int? tokenId, int pageSize, bool direction, int sortColumn, int pageNumber)
        {
            throw new NotImplementedException();
        }
    }
}