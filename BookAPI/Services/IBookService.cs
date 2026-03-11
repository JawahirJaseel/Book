using BookAPI.Models;

namespace BookAPI.Services;

public interface IBookService
{
    IEnumerable<Book> GetAllBooks();
    Book? GetBookById(Guid id);
    Book AddBook(Book book);
    bool UpdateBook(Guid id, Book updatedBook);
    bool DeleteBook(Guid id);
}
