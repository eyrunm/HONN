using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
	[Required]
        public String FirstName{ get; set; }
	/// <summary>
	/// The last name of the friend
	/// </summary>
	[Required]
        public String LastName { get; set; }
	/// <summary>
	/// The email of the friend
	/// </summary>
	[Required]
        public String Email { get; set; }
	/// <summary>
	/// The address of the friend
	/// </summary>	
	[Required]
        public String Address { get; set; }
    }
}