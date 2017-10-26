using System;
using System.Collections.Generic;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services
{
    public class RecommendationService : IRecommendationService
    {
        private ILibraryRepository _repo;
        public RecommendationService(ILibraryRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID)
        {
            var recommendations = _repo.GetRecommendationsForUser(userID);
            return recommendations;
        }
    }
}