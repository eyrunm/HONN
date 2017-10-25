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
    }
}
