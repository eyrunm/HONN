using System;

namespace LibraryAPI.Models
{
    /// <summary>
	/// This is a entity class for storing loans for friends
	/// </summary>
    public class Lanasafn
    {
    /// <summary>
	/// The ID of the book
	/// </summary>
        public int bok_id { get; set; }
    /// <summary>
	/// The date of the loan
	/// </summary>
        public String bok_lanadagsetning { get; set; }
    }
}