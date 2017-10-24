using System;
using System.Collections.Generic;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Models
{    /// <summary>
	/// This is a entity class for storing information about friends
	/// </summary>
    public class Vinur
    {
    	/// <summary>
	/// The ID of the friend
	/// </summary>
        public int vinur_id { get; set; }

	/// <summary>
	/// The first name of the friend
	/// </summary>
        public String fornafn{ get; set; }
	/// <summary>
	/// The last name of the friend
	/// </summary>
        public String eftirnafn { get; set; }
	/// <summary>
	/// The email of the friend
	/// </summary>
        public String netfang { get; set; }
	/// <summary>
	/// The address of the friend
	/// </summary>
        public String heimilisfang { get; set; }

    ///<summary>
    /// A list of loans the friend already has
    /// </summary>
        public List<Lanasafn> lanasafn {get; set; }
    }
}