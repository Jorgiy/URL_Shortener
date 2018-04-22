using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public interface ITokenRepository
    {
        Token GetToken(string token);

        Token InsertNewToken(Token insertEntity);

        void RemoveToken(int tokenId);
    }
}