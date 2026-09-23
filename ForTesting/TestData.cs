using FamilyLibrary.Models;
using FamilyLibrary.ViewModels;

namespace FamilyLibrary.ForTesting
{
    public static class TestData
    {
        public static readonly BookListViewModel TestBookList = new()
        {
            Books = new List<Book>()
        {
            new(1, "Test book 1", "Test description 1", "Test author 1", DateOnly.MinValue, "Test table of contents 1"),
            new(2, "Test book 2", "Test description 2", "Test author 2", DateOnly.MinValue, "Test table of contents 2"),
            new(3, "Test book 3", "Test description 3", "Test author 3", DateOnly.MinValue, "Test table of contents 3")
        },
            TotalPages = 1,
            CurrentPage = 1,
            SearchType = SearchType.ByTitle
        };
    }
}
