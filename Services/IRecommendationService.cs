using System;
using System.Collections.Generic;
using LibraryAPI.Models.ViewModels;

namespace LibraryAPI.Services
{
    public interface IRecommendationService
    {
        IEnumerable<RecommendationViewModel> GetRecommendationsForUser(int userID);
    }
}