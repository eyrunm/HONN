using System;
using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class ReviewService : IReviewService
    {
        private ILibraryRepository _repo;
        public ReviewService(ILibraryRepository repo)
        {
            _repo = repo;
        }

        public ReviewViewModel AddReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            var review = _repo.AddReviewByUser(rating, userID, bookID);
            return review;
        }

        public IEnumerable<ReviewViewModel> GetAllReviewsByUser(int userID)
        {
            var reviews = _repo.GetAllReviewsByUser(userID);

            return reviews;
        }

        public ReviewViewModel GetReviewByUserForBook(int userID, int bookID)
        {
            var reviews = _repo.GetReviewByUserForBook(userID, bookID);

            return reviews;
        }
        public void DeleteReviewByUserForBook(int userID, int bookID)
        {
            _repo.DeleteReviewByUserForBook(userID, bookID);
        }

        public ReviewViewModel UpdateReviewByUser(RatingDTO rating, int userID, int bookID)
        {
            var review = _repo.UpdateReviewByUser(rating, userID, bookID);
            
            return review;
        }
    }
}
