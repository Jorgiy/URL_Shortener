using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URLShortener.Interfaces;

namespace URLShortener.Services
{

    public class LinkCreationResult : ILinkCreationResult
    {
        public string ShortLink { get; set; }
        public int LinkId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}