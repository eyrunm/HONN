using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Repositories;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api")]
    public class LibraryController : Controller
    {
        private ILibraryService _libService;

        public LibraryController(ILibraryService libService)
        {
              _libService = libService;
        }
        // GET api/books
        [HttpGet]
        [Route("books")]
        public IActionResult GetAllBooks()
        {
            var books = _libService.getAllBooks();
            return Ok( books);
        }
        // GET api/values/5
        [HttpGet("books/{book_id:int}")]
        public IActionResult GetBookByID(int book_id)
        {
            try{
                var bookItem =  _libService.getBookByID(book_id);
                return Ok(bookItem);
            }
            catch(ObjectNotFoundException e){
                Console.WriteLine("book not found");
                return StatusCode(404, e.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
