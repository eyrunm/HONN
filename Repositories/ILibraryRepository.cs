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
    }
}
