using FamilyLibrary.ForTesting;
using FamilyLibrary.Models;
using FamilyLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FamilyLibrary.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(TestData.TestBookList);
        }

        [HttpGet]
        public IActionResult Card(int id)
        {
            Book book = GetBook(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Book book = GetBook(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(int id, BookEditViewModel editForm)
        {
            var index = TestData.TestBookList.Books.FindIndex(b => b.Id == id);
            if (index == -1) return NotFound();

            var updatedBook = TestData.TestBookList.Books[index] with
            {
                Title = editForm.Title,
                Author = editForm.Author,
                ReleaseDate = editForm.ReleaseDate,
                Description = editForm.Description,
                TableOfContents = editForm.TableOfContents
            };

            TestData.TestBookList.Books[index] = updatedBook;

            return RedirectToAction(nameof(Card), new { id = id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Book book = GetBook(id);
            TestData.TestBookList.Books.Remove(book);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        private Book GetBook(int id)
        {
            return TestData.TestBookList.Books.First(b => b.Id == id);
        }
    }
}