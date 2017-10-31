using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface IBookRepository
    {
        // Book functions
        IEnumerable<BookViewModel> GetAllBooks(DateTime? LoanDate);
        BookDetailsViewModel GetBookByID(int book_id);
        Book AddNewBook(Book newBook);
        void DeleteBookByID(int bookID);
        Book UpdateBookByID(Book updatedBook, int bookID);
    }
}