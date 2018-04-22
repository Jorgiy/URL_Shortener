using System.Data.Entity;
using DataLayer.Models;

namespace DataLayer.DataContext
{
    
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(string configuration)
            : base(configuration)
        {
        }
       
        public DbSet<Link> Links { get; set; }
        public DbSet<TokenMapping> TokenMappings { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
