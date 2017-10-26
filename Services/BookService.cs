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

        public IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks()
        {
            var reviews = _repo.GetAllReviewsForAllBooks();
            return reviews;
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID)
        {
            var reviews = _repo.GetAllReviewsForBook(bookID);
            return reviews;
        }

        public BookDetailsViewModel getBookByID(int book_id)
        {
           var book = _repo.GetBookByID(book_id);
           return book;
        }

        public ReviewViewModel GetReviewForBookByUser(int bookID, int userID)
        {
            var review = _repo.GetReviewForBookByUser(bookID, userID);
            return review;
        }

        public void OnStart()
        {
            _repo.OnStart();
        }

        public Book UpdateBookByID(Book updatedBook, int bookID)
        {
            var book = _repo.UpdateBookByID(updatedBook, bookID);

            return book;
        }

        public ReviewViewModel UpdateReviewForBookByUser(RatingDTO rating, int bookID, int userID)
        {
            var review = _repo.UpdateReviewByUser(rating, userID, bookID);
            return review;
        }
    }
}
