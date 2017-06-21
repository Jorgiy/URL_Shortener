using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URLShortener.Interfaces;

namespace URLShortener.Services
{

    public class DisplayedLink : IDisplayedLink
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