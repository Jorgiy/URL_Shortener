using System.Collections.Generic;
using AspNet.Mvc.Grid.Sorting;
using CoreServices.Models;

namespace CoreServices.Interfaces
{
    public interface IUserDataCoreService
    {
        IEnumerable<DisplayedLinkResult> GetUserPaginatedLinks(string token, SortDirection direction, int sortColumn);
    }
}
