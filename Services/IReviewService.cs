using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IReviewService
    {
        IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID);
        ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID);
        ReviewViewModel GetReviewByUserForBook(int userID, int bookID);
        void DeleteReviewByUserForBook(int userID, int bookID);
        ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID);

        IEnumerable<ReviewViewModel> GetAllReviewsForAllBooks();
        IEnumerable<ReviewViewModel> GetAllReviewsForBook(int bookID);
    }
}
