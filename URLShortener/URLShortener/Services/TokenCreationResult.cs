using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URLShortener.Interfaces;

namespace URLShortener.Services
{

    public class TokenCreationResult : ITokenCreationResult
    {
        public string Cookie { get; set; }
        public int TokenId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public bool NewToken { get; set; }
    }
}