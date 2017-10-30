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
    public class BookServiceTests
    {
        private IBookService _bookService;
        private ILibraryRepository _repo;

        [TestInitialize]
        public void Setup(){
            /// This method is executed before every single test
            _repo = new MockLibraryRepository();
            _bookService = new BookService(_repo);
        }

        [TestMethod]
        public void getListOfBooks()
        {
            /// Arrange
            ///Act
            var books = _bookService.GetAllBooks(null);
            ///Assert
            Assert.IsNotNull(books);
        }

        [TestMethod]
        public void getListOfBooksByDate()
        {
            /// Arrange
            String Date = "2015-10-12";
            DateTime LoanDate = Convert.ToDateTime(Date);
            ///Act
            var books = _bookService.GetAllBooks(LoanDate);
            ///Assert   
            Assert.IsNotNull(books);
        }

        [TestMethod]
        public void addNewBook_validValues()
        {
            var Title ="Harry Potter and the sorcerer's stone";
            var FirstName = "J K";
            var LastName = "Rowling";
            var DatePublished = "26-06-1997";
            /// Arrange
            var model = new Book
			{
				ID = 4, 
                Title ="Harry Potter and the sorcerer's stone", 
                FirstName = "J K", LastName = "Rowling", 
                DatePublished = "26-06-1997", 
                ISBN = "123456789"
			};
            /// Act
            _repo.AddNewBook(model);
            /// Assert
            var newEntity = _bookService.GetAllBooks(null).Last();
			// Tests if values are valid
            Assert.IsNotNull(newEntity);
            Assert.AreEqual(Title, newEntity.Title);
            Assert.AreEqual(FirstName + " " + LastName, newEntity.Author);
            Assert.AreEqual(DatePublished, newEntity.DatePublished);

        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void addNewBook_invalidValues() 
        {
            /// Arrange
            var model = new Book
			{
				ID = 4, 
                FirstName = "J K", LastName = "Rowling", 
                DatePublished = "26-06-1997", 
                ISBN = "123456789"
			};
            /// Act
            _repo.AddNewBook(model);
        }

        [TestMethod]
        public void getBookDetailsById_validValues() 
        {
            var Title ="Harry Potter and the chamber of secrets";
            var FirstName = "J K";
            var LastName = "Rowling";
            var DatePublished = "02-07-1998"; 
            var ISBN = "234567890";

            /// Arrange
            /// Act
            var book = _bookService.getBookByID(2);
            /// Assert
            Assert.IsNotNull(book);
            Assert.AreEqual(Title, book.Title);
            Assert.AreEqual(FirstName + " " + LastName, book.Author);
            Assert.AreEqual(DatePublished, book.DatePublished);
            Assert.AreEqual(ISBN, book.ISBN);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void getBookDetailsById_invalidValues() 
        {
            /// Arrange
            /// Act
            var book = _bookService.getBookByID(5);
            /// Assert
            Assert.IsNull(book);
        }

        [TestMethod]
        public void deleteBookById_validValues()
        {
            /// Arrange
            var count = _bookService.GetAllBooks(null).Count();
            /// Act
            _bookService.DeleteBookByID(1);
            var afterCount = _bookService.GetAllBooks(null).Count();
            /// Assert
            Assert.AreEqual(count-1, afterCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void deleteBookById_invalidValues() 
        {
            /// Arrange
            /// Act
            _bookService.DeleteBookByID(5);
            var book = _bookService.getBookByID(5);
            /// Assert
            Assert.IsNull(book);
        }

        [TestMethod]
        public void updateBookById_validValues() 
        {
            /// Arrange
            var updateModel = new Book
			{
				Title = "Harry Potter and the sorcerer's stone",
                FirstName = "J.K.", LastName = "Rowling", 
                DatePublished = "26-06-1997", 
                ISBN = "1122334455"
			};
            /// Act
            _repo.UpdateBookByID(updateModel, 1);
            /// Assert
            var book = _bookService.getBookByID(1);

            Assert.AreEqual(book.Title, updateModel.Title);
            Assert.AreEqual(book.Author, updateModel.FirstName +" "+ updateModel.LastName);
            Assert.AreEqual(book.DatePublished, updateModel.DatePublished);
            Assert.AreEqual(book.ISBN, updateModel.ISBN);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void updateBookById_invalidValues() 
        {
            /// Arrange
            var updateModel = new Book
			{
                FirstName = "J K", LastName = "Rowling", 
                DatePublished = "26-06-1997", 
                ISBN = "123456789"
			};
            /// Act
            _repo.UpdateBookByID(updateModel, 4);
            _repo.UpdateBookByID(updateModel, 1);
        }
    }
}
