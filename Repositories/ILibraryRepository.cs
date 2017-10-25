using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
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
        ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID);
        IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID);

        // User functions
        IEnumerable<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int userId);
        void DeleteUserById(int userId);
        void AddNewUser(Friend newUser);
        Friend UpdateUserById(Friend updatedUser, int userId);
        IEnumerable<BookViewModel> GetBooksByUserId(int userId);
        void AddBookToUser(int userId, int bookId);
        void ReturnBook(int userId, int bookId);
        Loan UpdateLoan(Loan updatedLoan, int userId, int bookId);
        void OnStart();
    }
}
