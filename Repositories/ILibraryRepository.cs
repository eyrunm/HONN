using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface ILibraryRepository
    {
        // Book functions
        IEnumerable<BookViewModel> GetAllBooks();
        BookDetailsViewModel GetBookByID(int book_id);
        void AddNewBook(Book newBook);
        void DeleteBookByID(int bookID);
        Book UpdateBookByID(Book updatedBook, int bookID);
        
        // User functions
        IEnumerable<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int userId);
        void DeleteUserById(int userId);
        void AddNewUser(Friend newUser);
        Friend UpdateUserById(Friend updatedUser, int userId);
        IEnumerable<BookViewModel> GetBooksByUserId(int userId);
        void AddBookToUser(int userId, int bookId);
        void ReturnBook(int userId, int bookId);
    }
}
