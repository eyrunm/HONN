using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IBookService
    {
        IEnumerable<BookViewModel> getAllBooks ();
        BookDetailsViewModel getBookByID(int book_id);
        void AddNewBook(Book newBook);
        void DeleteBookByID(int bookID);
        
    }
}