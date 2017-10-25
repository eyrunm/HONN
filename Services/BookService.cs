﻿using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private ILibraryRepository _repo;
        public BookService(ILibraryRepository repo)
        {
            _repo = repo;
        }

        public void AddNewBook(Book newBook)
        {
            _repo.AddNewBook(newBook);
        }

        public void DeleteBookByID(int bookID)
        {
            _repo.DeleteBookByID(bookID);
        }

        public IEnumerable<BookViewModel> getAllBooks(){
            var books = _repo.GetAllBooks();
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
