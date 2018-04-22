using System.Collections.Generic;
using AspNet.Mvc.Grid.Sorting;
using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface ITokenMappingRepository
    {
        TokenMapping InsertNewMapping(TokenMapping insertEntity);

        bool HasUserLink(int tokenId, int linkId);

        IEnumerable<TokenMapping> GetFullLinkInformationByToken(string token, SortDirection sortDirection, int sortColumn);
    }
}