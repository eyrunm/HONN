using System;
using System.Collections.Generic;

namespace LibraryAPI.Models.EntityModels
{    /// <summary>
	/// This is a entity class for storing information about friends
	/// </summary>
    public class Friend
    {
    	/// <summary>
	/// The ID of the friend
	/// </summary>
        public int ID { get; set; }

	/// <summary>
	/// The first name of the friend
	/// </summary>
        public String FirstName{ get; set; }
	/// <summary>
	/// The last name of the friend
	/// </summary>
        public String LastName { get; set; }
	/// <summary>
	/// The email of the friend
	/// </summary>
        public String Email { get; set; }
	/// <summary>
	/// The address of the friend
	/// </summary>
        public String Address { get; set; }

    ///<summary>
    /// A list of loan ID's the friend already has
    /// </summary>
        //public List<Lanasafn> Loans {get; set; }
    }
}