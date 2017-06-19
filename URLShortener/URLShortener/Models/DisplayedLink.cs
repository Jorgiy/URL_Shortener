using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using URLShortener.Interfaces;

namespace URLShortener.Models
{
    public class DisplayedLink : IDisplayedLink
    {
        public string OriginalLink { get; set; }
        public string ShortedLink { get; set; }
        public DateTime CreationDate { get; set; }
        public long Follows { get; set; }
    }
}