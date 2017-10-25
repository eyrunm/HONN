using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        //private ILibraryService _libService;

        public UsersController()
        {
              //_libService = libService;
        }

        // GET api/users
       /* [HttpGet]
        [Route("")]
        public IActionResult GetAllUsers()
        {
            var users = _libService.getAllUsers();
            return Ok(users);
        }*/

        // GET api/values/5
        [HttpGet("/{id}")]
        public string Get(int id)
        {
            return "value";
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
