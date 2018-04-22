namespace CoreServices.Models
{
    public class LinkCreationResult
    {
        public string ShortLink { get; set; }

        public int LinkId { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}