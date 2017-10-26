using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
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
        Book UpdateBookByID(Book updatedBook, int bookID);
        IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks();
        IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID);
        ReviewViewModel GetReviewForBookByUser(int bookID, int userID);
        ReviewViewModel UpdateReviewForBookByUser(RatingDTO rating, int bookID, int userID);


        void OnStart();
    }
}