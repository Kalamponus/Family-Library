using FamilyLibrary.DTO;
using FamilyLibrary.Models;

namespace FamilyLibrary.Persistence
{
    public class BooksService
    {
        private const int PageSize = 10;

        private readonly BooksRepository _booksRepository;

        public BooksService(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<PagedResult<Book>> SearchAsync(string? searchQuery, SearchType searchType, int page)
        {
            string? query = string.IsNullOrWhiteSpace(searchQuery) ? null : searchQuery.Trim();

            int totalCount = await _booksRepository.CountAsync(query, searchType);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)PageSize));

            page = Math.Clamp(page, 1, totalPages);

            var books = (await _booksRepository.SearchBooksAsync(query, searchType, page, PageSize)).ToList();

            return new PagedResult<Book>(books, totalCount, page, PageSize);
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            return _booksRepository.GetByIdAsync(id);
        }

        public Task<bool> CreateAsync(BookData bookData)
        {
            return _booksRepository.InsertAsync(bookData);
        }

        public Task<bool> UpdateAsync(int id, BookData bookData)
        {
            return _booksRepository.UpdateAsync(id, bookData);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _booksRepository.DeleteAsync(id);
        }
    }
}
