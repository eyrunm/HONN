using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Repositories
{
    public interface ILibraryRepository
    {
        // User functions
        IEnumerable<UserViewModel> GetAllUsers(String LoanDate, int LoanDuration);
        UserViewModel GetUserById(int userId);
        void DeleteUserById(int userId);
        Friend AddNewUser(Friend newUser);
        Friend UpdateUserById(Friend updatedUser, int userId);
        IEnumerable<BookViewModel> GetBooksByUserId(int userId);
        void AddBookToUser(int userId, int bookId);
        void ReturnBook(int userId, int bookId);
        Loan UpdateLoan(Loan updatedLoan, int userId, int bookId);

        IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID);

        void OnStart();

        // Review functions
        ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID);
        IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID);
        ReviewViewModel GetReviewByUserForBook(int userID, int bookID);
        ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID);
        ReviewViewModel GetReviewForBookByUser(int bookID, int userID);
        void DeleteReviewByUserForBook(int userID, int bookID);
        IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks();
        IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID);
    }
}
