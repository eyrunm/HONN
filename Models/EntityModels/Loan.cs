using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
	/// This is a entity class for storing loans for friends
	/// </summary>
    public class Loan
    {
    /// <summary>
	/// The ID of the Loan
	/// </summary>
        public int ID { get; set; }

    /// <summary>
	/// The ID of the book borrowed
	/// </summary>
    	[Required]
        public int bookID { get; set; }

	/// <summary>
	/// The ID of the friend
	/// </summary>
    	[Required]
        public int friendID { get; set; }
    /// <summary>
	/// The date of the loan
	/// </summary>
    	[Required]
        public DateTime DateBorrowed { get; set; }
    
    /// <summary>
	/// The date of the loan
	/// </summary>
        public DateTime? DateReturned { get; set; }

    /// <summary>
	/// The book has been returned
	/// </summary>
        public bool hasReturned { get; set; }
    }
}