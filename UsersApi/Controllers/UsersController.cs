using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Models;

namespace UsersApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User{Id = 1, Location = "NY", FirstName = "Peter", LastName = "Parker"}
        };
        
        [HttpGet(Name = "get-users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(users);
        }
        
        [HttpGet("{id}", Name = "get-user-by-id")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var foundUser = users.Find(user => user.Id == id);
            if (foundUser is null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        
        [HttpPost(Name = "create-user")]
        public async Task<ActionResult<User>> AddUser([FromBody] User newUser)
        {
            users.Add(newUser);
            return StatusCode(201, newUser.Id);
        }
        
        [HttpPut(Name = "update-user")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User updatedUser)
        {
            var foundUser = users.Find(user => user.Id == updatedUser.Id);
            if (foundUser is null)
            {
                return NotFound($"User with id {updatedUser.Id} not found");
            }

            foundUser.FirstName = updatedUser.FirstName;
            foundUser.LastName = updatedUser.LastName;
            foundUser.Location = updatedUser.Location;
            
            return Ok(foundUser);
        }
        
        [HttpDelete("{id}", Name = "delete-user")]
        public async Task<ActionResult<string>> DeleteUser(int id)
        {
            var foundUser = users.Find(user => user.Id == id);
            if (foundUser is null)
            {
                return NotFound($"User with id {id} not found");
            }

            users.Remove(foundUser);
            
            return Ok($"User with id {id} was removed");
        }
    }
}