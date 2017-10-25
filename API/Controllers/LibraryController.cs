using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api")]
    public class LibraryController : Controller
    {
        private IUserService _userService;
        private IBookService _bookService;
        private IRecommendationService _recommendationService;
        private IReportingService _reportingService;
        private IReviewService _reviewService;

        public LibraryController(IUserService userService, IBookService bookService, IRecommendationService recommendationService,
                                IReportingService reportingService, IReviewService reviewService)
        {
              _userService = userService;
              _bookService = bookService;
              _recommendationService = recommendationService;
              _reportingService = reportingService;
              _reviewService = reviewService;
        }
        // GET api/books
        [HttpGet]
        [Route("books")]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.getAllBooks();
            return Ok( books);
        }
        // GET api/books/5
        [HttpGet("books/{bookID:int}")]
        public IActionResult GetBookByID(int bookID)
        {
            try{
                var bookItem =  _bookService.getBookByID(bookID);
                return Ok(bookItem);
            }
            catch(ObjectNotFoundException e){
                Console.WriteLine("book not found");
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Creates a new book 
        /// </summary>
        /// <param name="newBook">A model for the new book</param>
        // POST api/books
        [HttpPost]
        [Route("books")]
        public IActionResult AddNewBook([FromBody]Book newBook)
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _bookService.AddNewBook(newBook);
                return StatusCode(201);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Updates a book 
        /// </summary>
        /// <param name="bookID">An integer id for the book</param>
        /// <param name="updatedBook">The updated values for the book</param>
        // PUT api/books/5
        [HttpPut("books/{bookID:int}")]
        public IActionResult UpdateBookByID([FromBody] Book updatedBook, int bookID)
        {
            if (updatedBook == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return StatusCode(412); }
            try{
                var book =  _bookService.UpdateBookByID(updatedBook, bookID);
                return Ok(book);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        // DELETE api/books/5
        [HttpDelete("books/{bookID:int}")]
        public IActionResult DeleteBookByID(int bookID)
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _bookService.DeleteBookByID(bookID);
                return StatusCode(204);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }
    }
}
