using FamilyLibrary.DTO;
using FamilyLibrary.Models;
using FamilyLibrary.Services;
using FamilyLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FamilyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchQuery, SearchType searchType = SearchType.ByTitle, int page = 1)
        {
            PagedResult<Book> books = await _booksService.SearchAsync(searchQuery, searchType, page);

            var model = new BookListViewModel
            {
                Books = books.Items,
                SearchQuery = searchQuery,
                SearchType = searchType,
                CurrentPage = books.Page,
                TotalPages = books.TotalPages
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Card(int id)
        {
            Book? book = await _booksService.GetByIdAsync(id);

            if (book is null)
                return NotFound();

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new BookFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookFormViewModel editForm)
        {
            if (!ModelState.IsValid)
                return View(editForm);

            BookData bookData = new(editForm.Title, editForm.Description, editForm.Author, editForm.ReleaseDate!.Value, editForm.TableOfContents);
            bool isSuccessful = await _booksService.CreateAsync(bookData);

            if (!isSuccessful)
            {
                ModelState.AddModelError("", "Не удалось создать книгу");
                return View(editForm);
            }

            TempData["Message"] = "Книга успешно добавлена";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Book? book = await _booksService.GetByIdAsync(id);

            if (book is null)
                return NotFound();

            BookFormViewModel viewModel = new()
            {
                Title = book.Title,
                Description = book.Description,
                ReleaseDate = book.ReleaseDate,
                Author = book.Author,
                TableOfContents = book.TableOfContents
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookFormViewModel editForm)
        {
            if (!ModelState.IsValid)
            {
                return View(editForm);
            }

            BookData bookData = new(editForm.Title, editForm.Description, editForm.Author, editForm.ReleaseDate!.Value, editForm.TableOfContents);
            bool isSuccessful = await _booksService.UpdateAsync(id, bookData);

            if (!isSuccessful)
            {
                ModelState.AddModelError("", "Не удалось обновить — книга не найдена");
                return View(editForm);
            }

            TempData["Message"] = "Изменения сохранены";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Card), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isSuccessful = await _booksService.DeleteAsync(id);

            TempData["Message"] = isSuccessful ? "Книга удалена" : "Не удалось удалить — книга не найдена";
            TempData["MessageType"] = isSuccessful ? "success" : "error";

            return RedirectToAction(nameof(Index)); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            return View(statusCode ?? 500);
        }
    }
}