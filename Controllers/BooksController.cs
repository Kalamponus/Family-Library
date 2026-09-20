using FamilyLibrary.Models;
using Microsoft.AspNetCore.Mvc;

public class BooksController : Controller
{
	public IActionResult Index()
    {
        return View();
    }

    public IActionResult Card(int id)
    {
        Book book = new(id, "Test book", "Test description", "Test author", DateOnly.MinValue, "Test table of contents");
        return View(book);
    }

    public IActionResult Error()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Edit(int id)
    {
        Book book = new(id, "Test book", "Test description", "Test author", DateOnly.MinValue, "Test table of contents");
        return View(book);
    }

    public IActionResult Delete(int id)
    {
        return View();
    }
}
