using System;
using System.Collections.Generic;

namespace LibraryAPI.Models.ViewModels
{    /// <summary>
	/// This is a entity class for displaying User information
	/// </summary>
    public class UserViewModel
    {

	/// <summary>
	/// The name of the users
	/// </summary>
        public String Name{ get; set; }
	
	/// <summary>
	/// The email of the user
	/// </summary>
        public String Email { get; set; }
	/// <summary>
	/// The address of the user
	/// </summary>
        public String Address { get; set; }
    }
}