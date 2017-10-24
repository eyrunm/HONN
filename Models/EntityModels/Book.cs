using System;

namespace LibraryAPI.Models.EntityModels
{    /// <summary>
	/// This is a entity class for storing information about a book
	/// </summary>
    public class Book
    {
    /// <summary>
	/// The ID of the book
	/// </summary>
        public int ID { get; set; }

    /// <summary>
	/// The title of the book
	/// </summary>
        public String Title { get; set; }

    /// <summary>
	/// The first name of the book's author
	/// </summary>
        public String FirstName { get; set; }

    /// <summary>
	/// The last name of the book's author
	/// </summary>
        public String LastName { get; set; }

    /// <summary>
	/// The date the book was published
	/// </summary>
        public String DatePublished { get; set; }

    /// <summary>
	/// The ISBN number for the book
	/// </summary>
        public String ISBN { get; set; }
    }
}