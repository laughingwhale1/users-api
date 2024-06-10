using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using UsersApi.Models;

namespace UsersApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UsersController( DataContext dataContext )
        {
            _dataContext = dataContext;
        }
        
        [HttpGet(Name = "get-users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var sqlQuery = $"SELECT * from users";
            var sqlExecutionRes = await _dataContext.users.FromSqlRaw(sqlQuery).ToListAsync();
            return Ok(sqlExecutionRes);
        }
        
        [HttpGet("{id}", Name = "get-user-by-id")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var sqlQuery = $"SELECT * from users WHERE id = {id}";
            var sqlExecutionRes = await _dataContext.users.FromSqlInterpolated($"SELECT * from users WHERE id = {id}").FirstOrDefaultAsync();
           
            if (sqlExecutionRes is null)
            {
                return NotFound();
            }
            
            return Ok(sqlExecutionRes);
        }
        
        [HttpPost(Name = "create-user")]
        public async Task<ActionResult<User>> AddUser([FromBody] User newUser)
        {
            FormattableString sqlQuery = $"INSERT INTO users (first_name, last_name, location) VALUES ({newUser.FirstName}, {newUser.LastName}, {newUser.Location})";
            await _dataContext.Database.ExecuteSqlInterpolatedAsync(sqlQuery);
            var createdUser = await _dataContext.users
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync(u => u.FirstName == newUser.FirstName && u.LastName == newUser.LastName && u.Location == newUser.Location);

            return StatusCode(201, createdUser.Id);
        }
        
        [HttpPut(Name = "update-user")]
        public async Task<ActionResult<User>> UpdateUser([FromBody] User updatedUser)
        {
            var foundUser = await _dataContext.users.Where(u => u.Id == updatedUser.Id).FirstOrDefaultAsync();
            if (foundUser is null)
            {
                return NotFound($"User with id {updatedUser.Id} not found");
            }

            foundUser.FirstName = updatedUser.FirstName;
            foundUser.LastName = updatedUser.LastName;
            foundUser.Location = updatedUser.Location;

            await _dataContext.SaveChangesAsync();
            
            return Ok(foundUser);
        }
        
        [HttpDelete("{id}", Name = "delete-user")]
        public async Task<ActionResult<string>> DeleteUser(int id)
        {
            var foundUser = await _dataContext.users.Where(user => user.Id == id).FirstOrDefaultAsync();
            if (foundUser is null)
            {
                return NotFound($"User with id {id} not found");
            }

            FormattableString sqlQuery = $"DELETE FROM users WHERE id = {id}";
            await _dataContext.Database.ExecuteSqlInterpolatedAsync(sqlQuery);
            
            return NotFound($"User with id {id} was removed");
        }
    }
}