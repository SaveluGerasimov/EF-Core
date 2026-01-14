using HomeworkEFpractic.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkEFpractic.Repositories
{
    public class UserRepository
    {
        private readonly LibraryDbContext _context;
        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public User GetUserById(int id)
        {
            return _context.Users
                .Include(u => u.Books)
                .FirstOrDefault(u => u.Id == id);
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Books).ToList();
        }
        public void AddUser(params User[] users)
        {
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            var user = _context.Users.Include(u => u.Books).FirstOrDefault(u => u.Id==id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public void UpdateUser(int id, string newName)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.Name = newName;
                _context.SaveChanges();
            }
        }
        public void BorrowBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.Books).FirstOrDefault(u => u.Id == userId);
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (user != null && book != null)
            {
                user.Books.Add(book);
                _context.SaveChanges();
            }
        }
        public void ReturnBook(int userId, int bookId)
        {
            var user = _context.Users.Include(u => u.Books).FirstOrDefault(u => u.Id == userId);
            var book = user.Books.FirstOrDefault(b => b.Id == bookId);
            if (user != null && book != null)
            {
                user.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}
