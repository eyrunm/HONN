using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryAPI.Services;
using LibraryAPI.Repositories;
namespace LibraryAPI.Test
{
    [TestClass]
    public class UnitTest1
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
            String Date = "2017-10-12";
            DateTime LoanDate = Convert.ToDateTime(Date);
            ///Act
            var books = _bookService.GetAllBooks(LoanDate);
            ///Assert   
            Assert.IsNotNull(books);
        }
    }
}
