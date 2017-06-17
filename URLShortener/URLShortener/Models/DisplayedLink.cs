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
        [DisplayName("Оригинальная ссылка")]
        public string OriginalLink { get; set; }

        [DisplayName("Укороченная ссылка")]
        public string ShortedLink { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Количество переходов")]
        public long Follows { get; set; }
    }
}