using CoreServices.Models;

namespace CoreServices.Interfaces
{
    public interface ILinkCoreService
    {
        LinkCreationResult CreateLink(string url);

        string ReturnOriginalLink(string shortenLink);

        void IncrementFollows(string shortenLink);
    }
}
