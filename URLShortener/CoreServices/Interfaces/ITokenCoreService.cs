using System;
using CoreServices.Models;

namespace CoreServices.Interfaces
{
    public interface ITokenCoreService
    {
         TokenMappingResult MapLinkToToken(int linkId, DateTime date, string token);
    }
}
