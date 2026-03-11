using BookAPI.Models;

namespace BookAPI.Services;

public class BookService : IBookService
{
    private readonly List<Book> _books;

    public BookService()
    {
        _books = new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Isbn = "978-0743273565", PublicationDate = new DateTime(1925, 4, 10) },
            new Book { Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee", Isbn = "978-0060935467", PublicationDate = new DateTime(1960, 7, 11) },
            new Book { Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell", Isbn = "978-0451524935", PublicationDate = new DateTime(1949, 6, 8) }
        };
    }

    public IEnumerable<Book> GetAllBooks()
    {
        return _books;
    }

    public Book? GetBookById(Guid id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public Book AddBook(Book book)
    {
        if (book.Id == Guid.Empty)
        {
            book.Id = Guid.NewGuid();
        }
        _books.Add(book);
        return book;
    }

    public bool UpdateBook(Guid id, Book updatedBook)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == id);
        if (existingBook == null)
        {
            return false;
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.Isbn = updatedBook.Isbn;
        existingBook.PublicationDate = updatedBook.PublicationDate;
        
        return true;
    }

    public bool DeleteBook(Guid id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return false;
        }

        _books.Remove(book);
        return true;
    }
}
