using FamilyLibrary.Models;

namespace FamilyLibrary.ViewModels
{
    public class BookListViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyList<Book> Books { get; set; } = [];
        public string? SearchQuery { get; set; }
        public SearchType SearchType { get; set; }
    }
}
