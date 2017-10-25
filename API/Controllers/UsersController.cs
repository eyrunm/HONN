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
