using FamilyLibrary.DTO;
using FamilyLibrary.Models;
using FamilyLibrary.Persistence;
using FamilyLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FamilyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private const int PageSize = 10;
        private readonly BooksRepository _booksRepository;

        public BooksController(BooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchQuery, SearchType searchType = SearchType.ByTitle, int page = 1)
        {
            var (books, totalCount) = await _booksRepository.SearchBooksAsync(searchQuery, searchType, page, PageSize);

            var model = new BookListViewModel
            {
                Books = books.ToList(),
                SearchQuery = searchQuery,
                SearchType = searchType,
                CurrentPage = page,
                TotalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)PageSize))
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Card(int id)
        {
            Book? book = await _booksRepository.GetByIdAsync(id);

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

            BookData bookData = new(editForm.Title, editForm.Description, editForm.Author, editForm.ReleaseDate, editForm.TableOfContents);
            bool isSuccessful = await _booksRepository.InsertAsync(bookData);

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
            Book? book = await _booksRepository.GetByIdAsync(id);

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

            BookData bookData = new(editForm.Title, editForm.Description, editForm.Author, editForm.ReleaseDate, editForm.TableOfContents);
            bool isSuccessful = await _booksRepository.UpdateAsync(id, bookData);

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
            bool isSuccessful = await _booksRepository.DeleteAsync(id);

            TempData["Message"] = isSuccessful ? "Книга удалена" : "Не удалось удалить — книга не найдена";
            TempData["MessageType"] = isSuccessful ? "success" : "error";

            return RedirectToAction(nameof(Index)); 
        }

        [HttpGet]
        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}