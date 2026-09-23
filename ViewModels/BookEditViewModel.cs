namespace FamilyLibrary.ViewModels
{
    public class BookEditViewModel
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public DateOnly ReleaseDate { get; set; }
        public string Description { get; set; } = "";
        public string TableOfContents { get; set; } = "";
    }
}
