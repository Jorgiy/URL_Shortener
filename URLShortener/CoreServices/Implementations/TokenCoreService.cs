using System;
using CoreServices.Interfaces;
using CoreServices.Models;
using DataLayer.Interfaces;
using DataLayer.Models;
using NLog;

namespace CoreServices.Implementations
{
    public class TokenCoreService : ITokenCoreService
    {
        private readonly ITokenMappingRepository _tokenMappingRepository;

        private readonly ITokenRepository _tokenRepository;

        private readonly Logger _logger;

        public TokenCoreService(ITokenMappingRepository tokenMappingRepository, ITokenRepository tokenRepository)
        {
            _tokenMappingRepository = tokenMappingRepository;
            _tokenRepository = tokenRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public TokenMappingResult MapLinkToToken(int linkId, DateTime date, string token)
        {
            int tokenId = 0;
            string newGuid = null;
            bool createdNewToken = true;

            if (token != null)
            {
                var existingToken = _tokenRepository.GetToken(token);
                if (existingToken != null)
                {
                    tokenId = existingToken.Id;
                    newGuid = existingToken.TokenString;
                    createdNewToken = false;
                }
            }

            if (newGuid == null)
            {
                newGuid = Guid.NewGuid().ToString();
                
                try
                {
                    tokenId = _tokenRepository.InsertNewToken(new Token {TokenString = newGuid}).Id;
                }
                catch (Exception addTokenEx)
                {
                    _logger.Fatal($"Unable to insert new token in token's table {addTokenEx.Message}");
                    return new TokenMappingResult()
                    {
                        Success = false,
                        ErrorMessage = "Error occured, link has not added to \"My links\""
                    };
                }
            }
            
            if (!_tokenMappingRepository.HasUserLink(tokenId, linkId))
            {
                try
                {
                    _tokenMappingRepository.InsertNewMapping(new TokenMapping
                    {
                        CreationDateTime = date,
                        LinkId = linkId,
                        TokenId = tokenId
                    });
                }
                catch (Exception linkTokenExc)
                {
                    _logger.Fatal($"Unable to link token with link {linkTokenExc.Message}");

                    if (createdNewToken)
                    {
                        try
                        {
                            _tokenRepository.RemoveToken(tokenId);
                        }
                        catch (Exception remTokExc)
                        {
                            _logger.Error(
                                $"Unable to remove token while failed mappiing this to link {remTokExc.Message}");
                        }
                    }


                    return new TokenMappingResult()
                    {
                        Success = false,
                        ErrorMessage = "Error occured, link has not added to \"My links\""
                    };
                }
            }

            return new TokenMappingResult()
            {
                TokenId = tokenId,
                Cookie = newGuid,
                Success = true,
                NewToken = createdNewToken
            };
        }
    }
}