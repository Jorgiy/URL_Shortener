using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    [Table("TOKEN_MAPPING")]
    public class TokenMapping
    {
        [Column("ID")]
        public long Id { get; set; }
        
        [Column("LINK_ID")]
        public int LinkId { get; set; }
        
        [Column("TOKEN_ID")]
        public int TokenId { get; set; }
        
        [Column("CREATION_DATE_TIME")]
        public DateTime CreationDateTime { get; set; }
    
        public virtual Link Link { get; set; }
        
        public virtual Token Token { get; set; }
    }
}
