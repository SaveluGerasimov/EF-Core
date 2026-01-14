using HomeworkEFpractic.Repositories;
using HomeworkEFpractic.Entities;
using HomeworkEFpractic;

namespace HomeworkEFpractic
{
    class Program
    {
        static void Main()
        {
            using (var db = new LibraryDbContext())
            {
                var userRepository = new UserRepository(db);
                var bookRepository = new BookRepository(db);

                var user1 = new User { Name = "пользователь 1", Email = "user1@gmail.com" };
                var user2 = new User { Name = "пользователь 2", Email = "user2@gmail.com" };
                userRepository.AddUser(user1, user2);

                var author = new Author { Name = "Джордж Оруэлл" };
                var author2 = new Author { Name = "Лев Толстой" };
                var author3 = new Author { Name = "Иван Тургенев" };
                db.Authors.AddRange(author, author2, author3);
                db.SaveChanges();

                var genre1 = new Genre { Name = "Антиутопия" };
                var genre2 = new Genre { Name = "Роман" };
                var genre3 = new Genre { Name = "Повесть" };
                db.Genres.AddRange(genre1, genre2, genre3);
                db.SaveChanges();

                var book1 = new Book { Title = "1984", Year = 1948, AuthorId = author.Id, GenreId = genre1.Id };
                var book2 = new Book { Title = "Война и мир", Year = 1863, AuthorId = author2.Id, GenreId = genre2.Id };
                var book3 = new Book { Title = "Муму", Year = 1852, AuthorId = author3.Id, GenreId = genre3.Id };
                var arraybooks = new [] { book1, book2, book3 };
                try
                {
                    for(var i = 0; i < arraybooks.Length; i++)
                    {
                        bookRepository.AddBook(arraybooks[i]);
                        Console.WriteLine($"Book {i+1} added sucsessfully");
                    }    
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error:{ex.Message}");
                }

                // Пользователь берет книгу на руки
                userRepository.BorrowBook(user1.Id, book1.Id);
                userRepository.BorrowBook(user2.Id, book3.Id);

                var users = userRepository.GetAllUsers().ToList();
                foreach (var us in users)
                {
                    Console.WriteLine($"Читатель с Id:{us.Id}, имя {us.Name}, почта {us.Email}");
                    foreach (var b in us.Books)
                    {
                        Console.WriteLine($"Выданная книга: {b.Title} автора {b.Author.Name}");
                    }
                }

                var books = bookRepository.GetAllBooks().ToList();
                foreach (var b in books)
                {
                    Console.WriteLine($"Книга {b.Id}: \"{b.Title}\", год {b.Year}");
                    foreach (var u in b.Users)
                    {
                        Console.WriteLine($"Выдана читателю: {u.Name}");
                    }
                }

                // Обновление имени пользователя
                userRepository.UpdateUser(user1.Id, "John Doe");

                // Обновление года выпуска книги
                bookRepository.UpdateBookYear(book2.Id, 2025);

                // Пользователь возвращает книгу
                userRepository.ReturnBook(user1.Id, book1.Id);
                userRepository.ReturnBook(user2.Id, book3.Id);

                // Удаление пользователя
                userRepository.DeleteUser(user1.Id);

                // Удаление книги
                bookRepository.DeleteBook(book1.Id);

                var booksByGenre = bookRepository.GetBooksByGenreAndYearRange(genre2.Id, 1800, 1900);
                Console.WriteLine("Получаем книги по жанру и годам:");
                foreach (var b in booksByGenre)
                {
                    Console.WriteLine($"{b.Title} ({b.Year})");
                }

                var bookCountByAuthor = bookRepository.GetBooksConutByAuthor(author.Id);
                Console.WriteLine($"Books by Author: {bookCountByAuthor}");

                var bookCountByGenre = bookRepository.GetBooksCountByGenre(genre2.Id);
                Console.WriteLine($"Books by Genre: {bookCountByGenre}");

                var bookExists = bookRepository.IsBookByAuthorAndTitleExists(author.Id, "1984");
                Console.WriteLine($"Book exists: {bookExists}");

                var bookBorrowed = bookRepository.IsBookBorrowedByUser(book2.Id, 2);
                Console.WriteLine($"Book borrowed by user: {bookBorrowed}");

                var borrowedBookCount = bookRepository.GetBorrowedBooksCountByUser(1);
                Console.WriteLine($"Borrowed books count: {borrowedBookCount}");

                var latestBook = bookRepository.GetLatestBook();
                Console.WriteLine($"Latest book: {latestBook?.Title} ({latestBook?.Year})");

                var booksAlphabetically = bookRepository.GetAllBooksSortedByAlphabet();
                Console.WriteLine("Books sorted alphabetically:");
                foreach (var b in booksAlphabetically)
                {
                    Console.WriteLine($"{b.Title}");
                }

                var booksByYearDescending = bookRepository.GetAllBooksSortedByYearDescending();
                Console.WriteLine("Books sorted by year descending:");
                foreach (var b in booksByYearDescending)
                {
                    Console.WriteLine($"{b.Title} ({b.Year})");
                }
            }
        }
    }
}
