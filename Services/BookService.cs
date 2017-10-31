using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _repo;
        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public Book AddNewBook(Book newBook)
        {
            var book = _repo.AddNewBook(newBook);
            if(book != null){
                return book;
            }
            else{
                return null;
            }
        }

        public void DeleteBookByID(int bookID)
        {
            _repo.DeleteBookByID(bookID);
        }

        public IEnumerable<BookViewModel> GetAllBooks(DateTime? LoanDate){
            var books = _repo.GetAllBooks(LoanDate);
            if(books != null){
                return books;
            }
            else{
                return null;
            }
        }

        public BookDetailsViewModel getBookByID(int book_id)
        {
           var book = _repo.GetBookByID(book_id);
           return book;
        }

        public Book UpdateBookByID(Book updatedBook, int bookID)
        {
            var book = _repo.UpdateBookByID(updatedBook, bookID);

            return book;
        }
    }
}
