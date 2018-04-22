using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    [Table("LINKS")]
    public class Link
    {
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("URL")]
        public string Url { get; set; }
        
        [Column("FOLLOWS")]
        public long Follows { get; set; }
        
        [Column("SHORT_URL")]
        public string ShortUrl { get; set; }
    }
}
