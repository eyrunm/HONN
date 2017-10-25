using System;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
	/// This is a entity class for storing book reviews(Ratings) by users
    /// Users can rate books from 0 - 5
	/// </summary>
    public class Review
    {
        
    /// <summary>
	/// The ID of the Review
	/// </summary>
        public int ID { get; set; }

    /// <summary>
	/// The ID of the book being rated
	/// </summary>
        public int bookID { get; set; }

	/// <summary>
	/// The ID of the friend
	/// </summary>
        public int friendID { get; set; }

    ///<summary>
    ///The Rating of the book
    /// a number between 1 and 5
    ///</summary>
        public int Rating { get; set; }

    }
}