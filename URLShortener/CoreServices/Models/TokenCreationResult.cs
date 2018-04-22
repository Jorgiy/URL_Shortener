namespace CoreServices.Models
{
    public class TokenMappingResult
    {
        public string Cookie { get; set; }
        
        public int TokenId { get; set; }
        
        public bool Success { get; set; }
        
        public bool NewToken { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}