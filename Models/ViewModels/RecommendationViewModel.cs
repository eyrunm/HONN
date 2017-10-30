using System;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
	/// This is a entity class for storing book reviews(Ratings) by users
    /// Users can rate books from 0 - 5
	/// </summary>
    public class RecommendationViewModel
    {

    /// <summary>
	/// The title of the book
	/// </summary>
        public String Title { get; set; }

    /// <summary>
	/// The name of the book's author
	/// </summary>
        public String Author { get; set; }

    /// <summary>
	/// The date the book was published
	/// </summary>
        public DateTime DatePublished { get; set; }

    /// <summary>
	/// The ISBN number for the book
	/// </summary>
        public String ISBN { get; set; }

    }
}