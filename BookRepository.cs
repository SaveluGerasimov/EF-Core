using HomeworkEFpractic.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkEFpractic.Repositories
{
    public class BookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public Book GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Users)
                .FirstOrDefault(b => b.Id == id);
        }
        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.Include(b => b.Author).Include(b => b.Genre).Include(b => b.Users).ToList();
        }
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        public void DeleteBook(int id)
        {
            var book = _context.Books.Include(b => b.Users).FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
        public void UpdateBookYear(int id, int newYear)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.Year = newYear;
                _context.SaveChanges();
            }
        }

        public List<Book> GetBooksByGenreAndYearRange(int genreId, int firstyear,  int lastyear)
        {
            return _context.Books
                .Where(b => b.GenreId == genreId && b.Year >= firstyear && b.Year <= lastyear)
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();
        }

        public int GetBooksConutByAuthor(int authorId)
        {
            return _context.Books
                .Where(b => b.AuthorId == authorId)
                .Count();
        }

        public int GetBooksCountByGenre(int genreId)
        {
            return _context.Books
                .Count(b => b.GenreId == genreId);
        }

        public bool IsBookByAuthorAndTitleExists(int authorId, string title)
        {
            return _context.Books.Any(b => b.AuthorId == authorId && b.Title == title);
        }

        public bool IsBookBorrowedByUser(int bookId, int userId)
        {
            return _context.Books
                .Include(b => b.Users)
                .Any(b => b.Id == bookId && b.Users.Any(u => u.Id == userId));
        }

        public int GetBorrowedBooksCountByUser(int userId)
        {
            return _context.Books
                .Include(b => b.Users)
                .Count(b => b.Users.Any(u => u.Id == userId) );
        }

        public Book GetLatestBook()
        {
            var latestBook = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Id)
                .FirstOrDefault() ?? throw new Exception("No books found in the database");
            return latestBook;
        }

        public List<Book> GetAllBooksSortedByAlphabet()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .OrderBy(b => b.Title)
                .ToList();
        }

        public List<Book> GetAllBooksSortedByYearDescending()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .OrderByDescending(b => b.Year)
                .ToList();
        }
    }
}
