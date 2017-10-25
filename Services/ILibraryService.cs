using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface ILibraryService
    {
        IEnumerable<BookViewModel> getAllBooks ();
        IEnumerable<UserViewModel> getAllUsers();
        BookDetailsViewModel getBookByID(int book_id);
        void AddNewBook(Book newBook);
    }
}