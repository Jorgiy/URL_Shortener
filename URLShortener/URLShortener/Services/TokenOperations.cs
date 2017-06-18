using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using URLShortener.DataContexts;
using URLShortener.Interfaces;
using URLShortener.Tools;
using static URLShortener.Tools.Sequences.SequencesTypes;

namespace URLShortener.Services
{
    public class TokenOperations : ITokenOperaions
    {
        public ITokenCreationResult CreateToken(int linkId, string token = null)
        {
            var db = new UrlShortenerBaseEntities();
            int nextUserId = 0;
            string newGuid = null;
            bool createdNewToken = true;

            if (token != null) // если нам передали токен, то проверим, есть ли действительно такой в базе
            {
                var existingToken = db.Tokens.FirstOrDefault(c => c.Token == token);
                if (existingToken != null)
                {
                    nextUserId = existingToken.Id;
                    newGuid = existingToken.Token;
                    createdNewToken = false;
                }
            }

            if (newGuid == null) // если токен по прежнему не найден, то сгененрируем его
            {
                try
                {
                    nextUserId = Sequences.GetNewId(NextUserId);
                }
                catch (Exception seqEx)
                {
                    Logger.LogAsync(ErrorType.Regular, $"Не удалось получить айди для нового токена. {seqEx.Message}",
                        DateTime.Now);
                    return new TokenCreationResult()
                    {
                        Success = false,
                        ErrorMessage = "Произошла ошибка, ссылка не была добавлена в \"Мои ссылки\""
                    };
                }

                newGuid = Guid.NewGuid().ToString();

                db.Tokens.Add(new Tokens {Id = nextUserId, Token = newGuid});

                try
                {
                    db.SaveChanges();
                }
                catch (Exception addTokenEx)
                {
                    Logger.LogAsync(ErrorType.Regular,
                        $"Не удалось вставить токен в таблицу токенов. {addTokenEx.Message}",
                        DateTime.Now);
                    return new TokenCreationResult()
                    {
                        Success = false,
                        ErrorMessage = "Произошла ошибка, ссылка не была добавлена в \"Мои ссылки\""
                    };
                }
            }

            db.TokenMapping.Add(new TokenMapping() {LinkId = linkId, TokenId = nextUserId});

            try
            {
                db.SaveChanges();
            }
            catch (Exception linkTokenExc)
            {
                Logger.LogAsync(ErrorType.Regular,
                        $"Не удалось удалить связать сущность токена и ссылки. {linkTokenExc.Message}",
                        DateTime.Now);

                try
                {
                    db.Tokens.Remove(db.Tokens.FirstOrDefault(c => c.Id == nextUserId));
                    db.SaveChanges();
                }
                catch (Exception remTokExc)
                {
                    Logger.LogAsync(ErrorType.Regular,
                        $"Не удалось удалить сущность токена после неудачной попытки связать токен и ссылку. {remTokExc.Message}",
                        DateTime.Now);
                }

                return new TokenCreationResult()
                {
                    Success = false,
                    ErrorMessage = "Произошла ошибка, ссылка не была добавлена в \"Мои ссылки\""
                };
            }

            return new TokenCreationResult()
            {
                TokenId = nextUserId,
                Cookie = newGuid,
                Success = true,
                NewToken = createdNewToken
            };
        }
    }

    public class TokenCreationResult : ITokenCreationResult
    {
        public string Cookie { get; set; }
        public int TokenId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public bool NewToken { get; set; }
    }
}