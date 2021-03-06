using System;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
	/// This is a entity class for storing book reviews(Ratings) by users
    /// Users can rate books from 0 - 5
	/// </summary>
    public class ReviewViewModel
    {

	/// <summary>
	/// The title of the book
	/// </summary>
        public String BookTitle { get; set; }

	/// <summary>
	/// The Author of the book
	/// </summary>
        public String AuthorFirstName { get; set; }

    /// <summary>
	/// The Author of the book
	/// </summary>
        public String AuthorLastName { get; set; } 


    ///<summary>
    ///The Rating of the book
    /// a number between 1 and 5
    ///</summary>
        public double Rating { get; set; }

    /// <summary>
	/// The date the book was published
	/// </summary>
        public String DatePublished { get; set; }

    }
}