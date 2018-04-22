using System.Linq;
using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Repositories
{
    public class TokensRepository : ITokenRepository
    {
        private readonly UrlShortenerContext _dbContext;

        public TokensRepository(UrlShortenerContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Token GetToken(string token)
        {
            return _dbContext.Tokens.SingleOrDefault(x => x.TokenString == token);
        }

        public Token InsertNewToken(Token insertEntity)
        {
            var inserted = _dbContext.Tokens.Add(insertEntity);
            _dbContext.SaveChanges();
            return inserted;
        }

        public void RemoveToken(int tokenId)
        {
            _dbContext.Tokens.Remove(_dbContext.Tokens.First(x => x.Id == tokenId));
        }
    }
}