using Dapper;
using FamilyLibrary.DTO;
using FamilyLibrary.Models;

namespace FamilyLibrary.Persistence
{
    public class BooksRepository
    {
        private readonly DapperContext _dapperContext;

        public BooksRepository(DapperContext context)
        {
            _dapperContext = context;
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string? searchQuery, SearchType searchType, int page, int pageSize)
        {
            int offset = (page - 1) * pageSize;

            var searchParameters = new { p_query = searchQuery, p_search_type = (int)searchType, p_limit = pageSize, p_offset = offset };

            using var connection = _dapperContext.CreateConnection();

            var books = await connection.QueryAsync<Book>("SELECT * FROM search_books(@p_query, @p_search_type, @p_limit, @p_offset);", searchParameters);

            return books;
        }

        public async Task<int> CountAsync(string? query, SearchType searchType)
        {
            var parameters = new { p_query = query, p_search_type = (int)searchType };

            using var connection = _dapperContext.CreateConnection();

            return await connection.ExecuteScalarAsync<int>("SELECT count_books(@p_query, @p_search_type);", parameters);
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var parameters = new {p_id = id};

            Book? book = await connection.QuerySingleOrDefaultAsync<Book>("SELECT * FROM get_book_by_id(@p_id::INT);", parameters);

            return book;
        }

        public async Task<bool> InsertAsync(BookData bookData)
        {
            var parameters = new {p_title = bookData.Title, p_description = bookData.Description, p_author = bookData.Author, p_release_date = bookData.ReleaseDate, p_table_of_contents = HtmlToXml.Normalize(bookData.TableOfContents)};
            string query = "CALL insert_book(@p_title::VARCHAR(255), @p_description::TEXT, @p_author::VARCHAR(255), @p_release_date::DATE, @p_table_of_contents::XML, NULL::INT);";

            using var connection = _dapperContext.CreateConnection();

            int result = await connection.ExecuteScalarAsync<int>(query, parameters);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(int id, BookData bookData)
        {
            var parameters = new {p_id = id, p_title = bookData.Title, p_description = bookData.Description, p_author = bookData.Author, p_release_date = bookData.ReleaseDate, p_table_of_contents = HtmlToXml.Normalize(bookData.TableOfContents)};
            string query = "CALL update_book(@p_id::INT, @p_title::VARCHAR(255), @p_description::TEXT, @p_author::VARCHAR(255), @p_release_date::DATE, @p_table_of_contents::XML, NULL::INT);";

            using var connection = _dapperContext.CreateConnection();

            int result = await connection.ExecuteScalarAsync<int>(query, parameters);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new { p_id = id };

            using var connection = _dapperContext.CreateConnection();

            int result = await connection.ExecuteScalarAsync<int>("CALL delete_book(@p_id::INT, NULL::INT);", parameters);
            return result > 0;
        }
    }
}
