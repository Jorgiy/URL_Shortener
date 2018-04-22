using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    [Table("TOKENS")]
    public class Token
    {
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("TOKEN")]
        public string TokenString { get; set; }
    }
}
