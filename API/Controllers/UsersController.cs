using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
              _userService = userService;
        }

        // GET api/users
        [HttpGet]
        [Route("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Gets the user with the given ID 
        /// </summary>
        /// <param name="userId">The ID for the user to be fetched</param>
        // GET api/user/1
        [HttpGet("users/{userId:int}")]
        public IActionResult GetUserById(int userId)
        {
            try{
                var user =  _userService.GetUserById(userId);
                return Ok(user);
            }
            catch(ObjectNotFoundException e){
                Console.WriteLine("User not found");
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Creates a new user 
        /// </summary>
        /// <param name="newUser">A model for the new user</param>
        // POST api/users
        [HttpPost("users")]
        public IActionResult AddNewUser([FromBody]Friend newUser)
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.AddNewUser(newUser);
                return StatusCode(201);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Updates a user 
        /// </summary>
        /// <param name="userId">An integer id for the user</param>
        /// <param name="updatedUser">The updated values for the user</param>
        // PUT api/users/5
        [HttpPut("users/{userId:int}")]
        public IActionResult UpdateUserById([FromBody] Friend updatedUser, int userId)
        {
            if (updatedUser == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return StatusCode(412); }
            try{
                var user =  _userService.UpdateUserById(updatedUser, userId);
                return Ok(user);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Deletes the user with the given id  
        /// </summary>
        /// <param name="userId">An integer id for the user</param>
        // DELETE api/users/1
        [HttpDelete("users/{userId:int}")]
        public IActionResult DeleteUserById(int userId)
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.DeleteUserById(userId);
                return StatusCode(204);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Gets a list of books that are registered to the user with given id
        /// </summary>
        /// <param name="userId">An integer id for the user</param>
        // GET api/users/5/books
        [HttpGet]
        [Route("users/{userId:int}/books")]
        public IActionResult GetBooksByUserId(int userId) 
        {
            try {
                 var books = _userService.GetBooksByUserId(userId);
                return Ok(books);
            }
            catch (ObjectNotFoundException e) {
                Console.WriteLine("User not found");
                return StatusCode(404, e.Message);
            }
        }

        [HttpPost]
        [Route("users/{userId:int}/books/{bookId:int}")]
        public IActionResult AddBookToUser(int userId, int bookId)
        {
             if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.AddBookToUser(userId, bookId);
                return StatusCode(201);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }
    }
}
