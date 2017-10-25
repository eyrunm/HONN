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

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            
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
