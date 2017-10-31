using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Services;
using LibraryAPI.Repositories;
using LibraryAPI.Models.EntityModels;

namespace LibraryAPI.Test
{
    [TestClass]
    public class UserServiceTests
    {
        private IUserService _userService;
        private ILibraryRepository _repo;

        [TestInitialize]
        public void Setup(){
            /// This method is executed before every single test
            _repo = new MockLibraryRepository();
            _userService = new UserService(_repo);
        }

        [TestMethod]
        public void getListOfUsers()
        {
            /// Arrange
            ///Act
            var users = _userService.GetAllUsers(null, 0);
            ///Assert
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void getListOfUsersByDate()
        {
            /// Arrange
            String Date = "2015-10-12";
            //DateTime LoanDate = Convert.ToDateTime(Date);
            ///Act
            var users = _userService.GetAllUsers(Date, 0);
            ///Assert   
            Assert.IsNotNull(users);
        }
        
        // Virkar ekki ! 
        /*[TestMethod]
        public void getListOfUsersWithLoanDuration()
        {
            /// Arrange
            int LoanDuration = 30;
            ///Act
            var users = _userService.GetAllUsers(null, LoanDuration);
            ///Assert   
            Assert.IsNotNull(users);
        }*/

        [TestMethod]
        public void addNewUser_validValues() 
        { 
            var FirstName = "Linda";
            var LastName = "Jóhansdóttir";
            var Email = "linda@linda.is";
            var Address = "Hæðarsel 1";
            /// Arrange
            var model = new Friend
			{
				ID = 5, 
                FirstName = "Linda", LastName = "Jóhansdóttir",
                Email = "linda@linda.is",
                Address = "Hæðarsel 1"
			};
            /// Act
            _repo.AddNewUser(model);
            /// Assert
            var newUser = _userService.GetAllUsers(null, 0).Last();
			// Tests if values are valid
            Assert.IsNotNull(newUser);
            Assert.AreEqual(FirstName + " " + LastName, newUser.Name);
            Assert.AreEqual(Email, newUser.Email);
            Assert.AreEqual(Address, newUser.Address);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void addNewUser_invalidValues() 
        {
            /// Arrange
            var model = new Friend
			{
				ID = 5, 
                Email = "linda@linda.is",
                Address = "Hæðarsel 1"
			};
            /// Act
            _repo.AddNewUser(model);
        }

        [TestMethod]
        public void getUserDetailsById_validValues() 
        { 
            var FirstName = "Sigga"; 
            var LastName = "Jóns";
            var Email = "sigga@sigga.is";
            var Address = "laugavegur 1";

            /// Arrange
            /// Act
            var user = _userService.GetUserById(1);
            /// Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(FirstName + " " + LastName, user.Name);
            Assert.AreEqual(Email, user.Email);
            Assert.AreEqual(Address, user.Address);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getUserDetailsById_invalidValues() 
        {
            /// Arrange
            /// Act
            var user = _userService.GetUserById(5);
            /// Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void deleteUserById_validValues()
        {
            /// Arrange
            var count = _userService.GetAllUsers(null, 0).Count();
            /// Act
            _userService.DeleteUserById(1);
            var afterCount = _userService.GetAllUsers(null, 0).Count();
            /// Assert
            Assert.AreEqual(count-1, afterCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void deleteUserById_invalidValues() 
        {
            /// Arrange
            /// Act
            _userService.DeleteUserById(5);
            var user = _userService.GetUserById(5);
            /// Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void updateUserById_validValues() 
        {
            /// Arrange
            var updateModel = new Friend
			{
                FirstName = "Sigga", LastName = "Sigurjónsdóttir",
                Email = "siggasig@siggasig.is",
                Address = "Austurstræti 10"
			};
            /// Act
            _repo.UpdateUserById(updateModel, 1);
            /// Assert
            var user = _userService.GetUserById(1);

            Assert.AreEqual(user.Name, updateModel.FirstName +" "+ updateModel.LastName);
            Assert.AreEqual(user.Email, updateModel.Email);
            Assert.AreEqual(user.Address, updateModel.Address);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void updateUserById_invalidValues() 
        {
            /// Arrange
            var updateModel = new Friend
			{
                FirstName = "Sigga", LastName = "Sigurjónsdóttir",
                Address = "Austurstræti 10"
			};
            /// Act
            _repo.UpdateUserById(updateModel, 4);
            _repo.UpdateUserById(updateModel, 1);
        }
    }
}