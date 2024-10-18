namespace LogTheDay_WebAPI.Controllers.Domain.Entities
{
    public class Page // dummy-class for pages
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string PageType { get; set; }
        public int PrivacyType { get; set; }
        public string CustomCss { get; set; }

    }
}
