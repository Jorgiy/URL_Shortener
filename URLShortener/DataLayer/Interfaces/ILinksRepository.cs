using DataLayer.Models;
using Utils;

namespace DataLayer.Interfaces
{
    public interface ILinksRepository
    {
        Link GetByUrl(string url);

        Link InsertLink(Link insertEntity, ConversionProvider conversionProvider);

        Link GetByShortUrl(string shortUrl);

        void IncrementFollows(string shortUrl);
    }
}