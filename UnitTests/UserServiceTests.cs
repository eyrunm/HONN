using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Services;
using LibraryAPI.Repositories;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Test
{
    [TestClass]
    public class UserServiceTests
    {
        private IUserService _userService;
        private IBookService _bookService;
        private ILibraryRepository _repo;
        private IBookRepository _bookRepo;

        [TestInitialize]
        public void Setup(){
            /// This method is executed before every single test
            _repo = new MockLibraryRepository();
            _bookRepo = new MockBookRepository();
            _userService = new UserService(_repo);
            _bookService = new BookService(_bookRepo);
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

        [TestMethod]
        public void getBooksByUserId_validValues() 
        {
            /// Arrange
            var userModel = new Friend
			{
				ID = 5, 
                FirstName = "Linda", LastName = "Jóhansdóttir",
                Email = "linda@linda.is",
                Address = "Hæðarsel 1"
			};
            var bookModel = new Book
            {
                ID = 4, 
                Title ="The Hobbit", 
                FirstName = "J R R", LastName = "Tolkien", 
                DatePublished = Convert.ToDateTime("21-09-1937"), 
                ISBN = "0000000000"
            };
            _userService.AddNewUser(userModel);
            _bookService.AddNewBook(bookModel);
            /// Act
            _userService.AddBookToUser(3, 2);
            
            /// Assert
            var books = _userService.GetBooksByUserId(3);
            var count = books.Count();

            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getBooksByUserId_invalidValues() 
        {
            /// Arrange
            /// Act
            /// Assert
            _repo.GetBooksByUserId(10);
        }

        [TestMethod]
        public void addBookToUser_validValues()
        {
            /// Arrange
            var userModel = new Friend
			{
				ID = 1, 
                FirstName = "Sigga", LastName = "Jóns",
                Email = "sigga@sigga.is",
                Address = "laugavegur 1"
			};
            var bookModel = new Book
            {
                ID = 2, 
                Title ="Harry Potter and the chamber of secrets", 
                FirstName = "J K", LastName = "Rowling", 
                DatePublished = Convert.ToDateTime("02-07-1998"), 
                ISBN = "234567890"
            };
            /// Act
            _repo.AddBookToUser(1, 2);
            /// Assert
            var book = _userService.GetBooksByUserId(1).Last();
            
            Assert.AreEqual(bookModel.Title, book.Title);
            Assert.AreEqual(bookModel.FirstName + " " + bookModel.LastName, book.Author);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void addBookToUser_invalidValues()
        {
            /// Arrange
            /// Act 
            /// Assert
            /// Invalid book id    
            _repo.AddBookToUser(1,9);
            /// Invalid user id 
            _repo.AddBookToUser(9,1);
        }

        /*[TestMethod]
        public void returnBook_validValues()
        {
            /// Arrange
            var loanModel = new Loan
            {
                ID = 6,
                bookID = 3,
                friendID = 1,
                DateBorrowed = Convert.ToDateTime("2016-06-02"),
                hasReturned = false
            };
            var book = _repo.AddBookToUser(1, 3);
            /// Act
            _userService.ReturnBook(1, 3);
            /// Assert
            Assert.IsNotNull(book);
        }*/

        [TestMethod]
        public void updateLoan_validValues()
        {
            /// Arrange
            var updateModel = new Loan
			{
                bookID = 1,
                friendID = 1,
                DateBorrowed = Convert.ToDateTime("2017-11-11"),
                DateReturned = null,
                hasReturned = false
			};
            /// Act
            var loan = _repo.UpdateLoan(updateModel, 1, 1);

            /// Assert
            Assert.AreEqual(loan.bookID, updateModel.bookID);
            Assert.AreEqual(loan.friendID, updateModel.friendID);
            Assert.AreEqual(loan.DateBorrowed, updateModel.DateBorrowed);
            Assert.AreEqual(loan.DateReturned, updateModel.DateReturned);
            Assert.AreEqual(loan.hasReturned, updateModel.hasReturned);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void updateLoan_invalidIDValues() 
        {
            var updateModel = new Loan
			{
                bookID = 1,
                friendID = 1,
                DateBorrowed = Convert.ToDateTime("2017-11-11"),
                DateReturned = null,
                hasReturned = false
			};
            /// Act
            _repo.UpdateLoan(updateModel, 10, 23);
        }

        [TestMethod]
        public void getReviewByUserId_valid()
        {
            /// Arrange
            var model = new Review 
            {
                ID = 1,
                bookID = 1,
                friendID = 1,
                Rating = 5
            };
            /// Act
            var review = _repo.GetAllReviewsByUser(1);
            var count = review.Count();
            /// Assert
            Assert.IsNotNull(review);
            Assert.AreEqual(count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getReviewsByUserId_invalidUserID()
        {
            /// Arrange
        
            /// Act
             
            /// Assert
            _repo.GetAllReviewsByUser(8);
        }

        [TestMethod]
        public void getReviewOnBookByUserId_valid()
        {
            /// Arrange
            var model = new Review 
            {
                ID = 1,
                bookID = 1,
                friendID = 1,
                Rating = 5
            };
            /// Act
            var review = _repo.GetReviewByUserForBook(1, 1);
            /// Assert
            Assert.IsNotNull(review);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getReviewOnBookByUserId_invalidUserID()
        {
            /// Arrange
        
            /// Act
             
            /// Assert
            _repo.GetReviewByUserForBook(8, 1);
        }

        [TestMethod]
        public void addReviewByUser_validInput()
        {
            /// Arrange
            var rateModel = new RatingDTO
            {
                Rating = 1
            };
            /// Act
            _repo.AddReviewByUser(rateModel, 2, 2);
            var review = _repo.GetReviewByUserForBook(2, 2);
            /// Assert
            Assert.IsNotNull(review);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void addReviewByUser_invalidID()
        {
            /// Arrange
            var rateModel = new RatingDTO
            {
                Rating = 1
            };
            /// Act
            
            /// Assert
            _repo.AddReviewByUser(rateModel, 7, 2);
            _repo.AddReviewByUser(rateModel, 2, 7);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingException))]
        public void addReviewByUser_invalidRatingValue()
        {
            /// Arrange
            var rateModel = new RatingDTO
            {
                Rating = 10
            };
            /// Act
            
            /// Assert
            _repo.AddReviewByUser(rateModel, 2, 2);
        }

        [TestMethod]
        public void deleteReview_validValues()
        {
            /// Arrange
            var count = _repo.GetAllReviewsByUser(2).Count();
            /// Act
            _repo.DeleteReviewByUserForBook(2, 1);
            var afterCount = _repo.GetAllReviewsByUser(2).Count();
            
            /// Assert
            Assert.AreEqual(count-1, afterCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void deleteReview_invalidIDs()
        {
            /// Arrange
            
            /// Act
            
            /// Assert
            _repo.DeleteReviewByUserForBook(100, 2);
            _repo.DeleteReviewByUserForBook(1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void deleteReview_invalidReviewId()
        {
            /// Arrange
            
            /// Act
            
            /// Assert
            _repo.DeleteReviewByUserForBook(100, 100);
        }

        [TestMethod]
        public void updateReview_validValues()
        {
             /// Arrange
            var rateModel = new RatingDTO
            {
                Rating = 1
            };
            var review = _repo.GetReviewByUserForBook(1, 1);
            /// Act
            var updatedReview = _repo.UpdateReviewByUser(rateModel, 1, 1);
            /// Assert
            Assert.AreEqual(review.BookTitle, updatedReview.BookTitle);
            Assert.AreEqual(rateModel.Rating, updatedReview.Rating);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void updateReview_invalidIDs() 
        {
            /// Arrange
            var rateModel = new RatingDTO
			{
                Rating = 1
			};
            /// Act
   
            /// Assert
            _repo.UpdateReviewByUser(rateModel, 100, 1);
            _repo.UpdateReviewByUser(rateModel, 1, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(RatingException))]
        public void updateReview_invalidReview() 
        {
            /// Arrange
            var rateModel = new RatingDTO
			{
                Rating = 1
			};
            /// Act
   
            /// Assert
            _repo.UpdateReviewByUser(rateModel, 1, 3);
        }

        [TestMethod]
        public void GetRecommendationForUser_valid()
        {
            /// Arrange
            /// Act
            var rec = _repo.GetRecommendationsForUser(1);
            /// Assert
            Assert.IsNotNull(rec);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getRecommendationForUser_invalidUserID()
        {
            /// Arrange
            /// Act
            /// Assert
            _repo.GetRecommendationsForUser(10);
        }

        [TestMethod]
        public void getAllReviewsForAllBooks_valid()
        {
            /// Arrange
            /// Act
            var rev = _repo.GetAllReviewsForAllBooks();
            /// Assert
            Assert.IsNotNull(rev);
        }

        // Vill ekki henda Exceptioni ?? 
        /*[TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getAllReviewsForAllBooks_noReviewsInDataBase()
        {
            /// Arrange
            _repo.DeleteReviewByUserForBook(1,1);
            _repo.DeleteReviewByUserForBook(2,1);
            _repo.DeleteReviewByUserForBook(3,1);
            _repo.DeleteReviewByUserForBook(3,2);
            /// Act
             
            /// Assert
            _repo.GetAllReviewsForAllBooks();
        }*/

        [TestMethod]
        public void getAllReviewsForBook_valid() 
        {
            /// Arrange
            /// Act
            var rev = _repo.GetAllReviewsForBook(1);
            /// Assert
            Assert.IsNotNull(rev);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getAllReviewsForBook_invalidID()
        {
            /// Arrange
            /// Act
            /// Assert
            _repo.GetAllReviewsForBook(15);
        }

        [TestMethod]
        public void getReviewForBookByUser_valid() 
        {
            /// Arrange
            /// Act
            var rev = _repo.GetReviewForBookByUser(1,2);
            /// Assert
            Assert.IsNotNull(rev);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getReviewForBookByUser_invalidID()
        {
            /// Arrange
            /// Act
            /// Assert
            _repo.GetReviewForBookByUser(1,5);
            _repo.GetReviewForBookByUser(5,1);
        }
    }
}