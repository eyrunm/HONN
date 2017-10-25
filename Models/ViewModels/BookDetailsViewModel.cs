using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Models.ViewModels
{    /// <summary>
	/// This is a entity class for viewing detailed information about a book
	/// </summary>
    public class BookDetailsViewModel
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
        public String DatePublished { get; set; }

    /// <summary>
	/// The ISBN number for the book
	/// </summary>
        public String ISBN { get; set; }

    /// <summary>
	/// The Average rating of the book
	/// </summary>
        public double Rating { get; set; }

    /// <summary>
	/// The loan history for the book
	/// </summary>
        public List<Loan> loanHistory { get; set; }

    
    }
}