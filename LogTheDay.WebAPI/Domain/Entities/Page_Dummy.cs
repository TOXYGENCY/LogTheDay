namespace LogTheDay.Controllers.Domain.Entities
{
    public class Page // dummy-class for pages
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string PageType { get; set; }
        public int PrivacyType { get; set; }
        public string CustomCss { get; set; }

    }
}
