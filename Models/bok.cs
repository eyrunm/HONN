using System;

namespace LibraryAPI.Models
{    /// <summary>
	/// This is a entity class for storing information about a book
	/// </summary>
    public class Bok
    {
    /// <summary>
	/// The ID of the book
	/// </summary>
        public int bok_id { get; set; }

    /// <summary>
	/// The title of the book
	/// </summary>
        public String bok_titill { get; set; }

    /// <summary>
	/// The first name of the book's author
	/// </summary>
        public String fornafn_hofundar { get; set; }

    /// <summary>
	/// The last name of the book's author
	/// </summary>
        public String eftirnafn_hofundar { get; set; }

    /// <summary>
	/// The date the book was published
	/// </summary>
        public String utgafudagur { get; set; }

    /// <summary>
	/// The ISBN number for the book
	/// </summary>
        public String ISBN { get; set; }
    }
}