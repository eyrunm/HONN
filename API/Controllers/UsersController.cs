using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models.DTOModels;
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
        private IReviewService _reviewService;
        private IRecommendationService _recommendationService;

        public UsersController(IUserService userService, IReviewService reviewService, IRecommendationService recommendationService)
        {
              _userService = userService;
              _reviewService = reviewService;
              _recommendationService = recommendationService;
              _userService.OnStart();
        }
        /// <summary>
        /// Returns all users in the system 
        /// </summary>
        // GET api/users
        [HttpGet]
        [Route("users")]
        public IActionResult GetAllUsers(String LoanDate = "", int LoanDuration = 0)
        {
            var users = _userService.GetAllUsers(LoanDate, LoanDuration);
            return Ok(users);
        }

        /// <summary>
        /// Gets the user with the given ID 
        /// </summary>
        /// <param name="userID">The ID for the user to be fetched</param>
        // GET api/user/1
        [HttpGet("users/{userID:int}")]
        public IActionResult GetUserByID(int userID)
        {
            try{
                var user =  _userService.GetUserById(userID);
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
        /// <param name="userID">An integer id for the user</param>
        /// <param name="updatedUser">The updated values for the user</param>
        // PUT api/users/5
        [HttpPut("users/{userID:int}")]
        public IActionResult UpdateUserById([FromBody] Friend updatedUser, int userID)
        {
            if (updatedUser == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return StatusCode(412); }
            try{
                var user =  _userService.UpdateUserById(updatedUser, userID);
                return Ok(user);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Deletes the user with the given id  
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        // DELETE api/users/1
        [HttpDelete("users/{userID:int}")]
        public IActionResult DeleteUserByID(int userID)
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.DeleteUserById(userID);
                return StatusCode(204);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Gets a list of books that are registered to the user with given id
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        // GET api/users/5/books
        [HttpGet]
        [Route("users/{userID:int}/books")]
        public IActionResult GetBooksByUserID(int userID) 
        {
            try {
                var books = _userService.GetBooksByUserId(userID);
                return Ok(books);
            }
            catch (ObjectNotFoundException e) {
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Adds a book with given id to a user with given id
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        // POST api/users/1/books/18
        [HttpPost]
        [Route("users/{userID:int}/books/{bookID:int}")]
        public IActionResult AddBookToUser(int userID, int bookID)
        {
             if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.AddBookToUser(userID, bookID);
                return StatusCode(201);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Returns the book with the given ID
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        // DELETE api/users/1/books/18
        [HttpDelete]
        [Route("users/{userID:int}/books/{bookID:int}")]
        public IActionResult ReturnBook(int userID, int bookID) 
        {
            if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                _userService.ReturnBook(userID, bookID);
                return StatusCode(204);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(412, e.Message);
            }
        }

        /// <summary>
        /// Updates the loan with new information
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        /// <param name="updatedLoan">The updated values for the loan</param>
        // PUT api/users/1/books/18
        [HttpPut]
        [Route("users/{userID:int}/books/{bookID:int}")]
        public IActionResult UpdateLoan([FromBody] Loan updatedLoan, int userID, int bookID) 
        {
            if (updatedLoan == null) { return BadRequest(); }
            if (!ModelState.IsValid) { return StatusCode(412); }
            try {
                var loan =  _userService.UpdateLoan(updatedLoan, userID, bookID);
                return Ok(loan);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        ///Review related functions


        /// <summary>
        /// Returns all Reviews by the user with the given ID
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        [HttpGet]
        [Route("users/{userID:int}/reviews")]
        public IActionResult GetAllReviewsByUser(int userID){
            try{
                var reviews =  _reviewService.GetAllReviewsByUser(userID);
                return Ok(reviews);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
            catch (RatingException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Posts a new review for a book with the given BookID by the user
        /// with the given userID
        /// </summary>
        /// <param name="rating">RatingDTO object containing the rating to be added</param>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        // POST api/users/1/books/18
        [HttpPost]
        [Route("users/{userID:int}/reviews/{bookID:int}")]
        public IActionResult AddReviewByUser([FromBody] RatingDTO rating, int userID, int bookID)
        {
             if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                var review =  _reviewService.AddReviewByUser(rating, userID, bookID);
                return Ok(review);
            }
            catch (RatingException e){
                return StatusCode(404, e.Message);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Returns Review by the user with the given ID for 
        ///a book with bookID
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        [HttpGet]
        [Route("users/{userID:int}/reviews/{bookID:int}")]
        public IActionResult GetReviewByUserForBook(int userID, int bookID){
            try{
                var reviews =  _reviewService.GetReviewByUserForBook(userID, bookID);
                return Ok(reviews);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
            catch (RatingException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Deletes review made by the user with the given ID
        /// for a book with the given ID
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        [HttpDelete]
        [Route("users/{userID:int}/reviews/{bookID:int}")]
        public IActionResult DeleteReviewByUserForBook(int userID, int bookID){
            try{
                _reviewService.DeleteReviewByUserForBook(userID, bookID);
                return NoContent();
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
            catch (RatingException e){
                return StatusCode(404, e.Message);
            }
        }
        /// <summary>
        /// Updates an existing review made by the user
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        /// <param name="bookID">An integer id for the book</param>
        // POST api/users/1/books/18
        [HttpPut]
        [Route("users/{userID:int}/reviews/{bookID:int}")]
        public IActionResult UpdateReviewByUser([FromBody] RatingDTO rating, int userID, int bookID)
        {
             if(!ModelState.IsValid){
                return StatusCode(412);
            }
            try{
                var review =  _reviewService.UpdateReviewByUser(rating, userID, bookID);
                return Ok(review);
            }
            catch (RatingException e){
                return StatusCode(404, e.Message);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

        /// <summary>
        /// Returns recommendations for the user with the given ID
        /// </summary>
        /// <param name="userID">An integer id for the user</param>
        [HttpGet]
        [Route("users/{userID:int}/recommendation")]
        public IActionResult GetRecommendationsForUser(int userID){
            try{
                var recommendations =  _recommendationService.GetRecommendationsForUser(userID);
                return Ok(recommendations);
            }
            catch(ObjectNotFoundException e){
                return StatusCode(404, e.Message);
            }
        }

    }
}
