using BookAPI.Models;
using BookAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        return Ok(_bookService.GetAllBooks());
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(Guid id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> CreateBookItem([FromBody] Book book)
    {
        var addedBook = _bookService.AddBook(book);
        return CreatedAtAction(nameof(GetBook), new { id = addedBook.Id }, addedBook);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBookItem(Guid id, [FromBody] Book book)
    {
        var updated = _bookService.UpdateBook(id, book);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBookItem(Guid id)
    {
        var deleted = _bookService.DeleteBook(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
