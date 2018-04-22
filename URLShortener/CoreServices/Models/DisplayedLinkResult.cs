using System;
using System.ComponentModel.DataAnnotations;

namespace CoreServices.Models
{
    public class DisplayedLinkResult
    {
        public string OriginalLink { get; set; }
        
        public string ShortedLink { get; set; }
        
        public DateTime CreationDate { get; set; }
        
        public long Follows { get; set; }
    }
}