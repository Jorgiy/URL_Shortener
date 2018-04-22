using System;
using System.ComponentModel.DataAnnotations;

namespace CoreServices.Models
{
    public class DisplayedLinkResult
    {
        [Display(Name = "Оригинальная ссылка")]
        public string OriginalLink { get; set; }
        
        [Display(Name = "Укороченная ссылка")]
        public string ShortedLink { get; set; }
        
        [Display(Name = "Дата создания")]
        public DateTime CreationDate { get; set; }
        
        [Display(Name = "Количество переходов")]
        public long Follows { get; set; }
    }
}