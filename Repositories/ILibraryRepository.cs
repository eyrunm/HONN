using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface ILibraryRepository
    {
        IEnumerable<BookViewModel> getAllBooks();
        BookDetailsViewModel getBookByID(int book_id);
        IEnumerable<UserViewModel> getAllUsers();
        void AddNewBook(Book newBook);
    }
}
