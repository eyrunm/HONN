using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
	/// This is a entity class for posting Rating by user
    /// Users can rate books from 0 - 5
	/// </summary>
    public class RatingDTO
    {

    ///<summary>
    ///The Rating of the book
    /// a number between 1 and 5
    ///</summary>
        public int Rating { get; set; }

    }
}