using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/*
* An API Controller inherits from a controller base class
* An API Controller has an API Controller Attribute
* An API Controller has different endpoints
* And we return action results from our API controllers
*/

namespace API.Controllers // Defines the namespace for organizing code and preventing naming conflicts.
{
    [ApiController] 
    [Route("api/[controller]")] 

    // (DataContext context) is a cleaner way of writing the constructor from C# 12 onwards
    public class UsersController(DataContext context) : BaseApiController // Class UsersController is deriving from BaseApiController which derives 
                                                                       // class ControllerBase
    {
        // This is an HTTP get request with method inside it to create a response
        [HttpGet]
        // IEnumerable is a type of list used for a collection of a specified type
        // GetUsers() is an API endpoint
        public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers() // async Task<> has been added to make the code asynchronous
        {
            // await is a keyword needed for async methods and ToListAsync() is an async version of ToList();
            var users = await context.Users.ToListAsync(); // Retrieves all users from the database and converts them into a list.

            // When using ActionResult, we have an option to return HTTP responses
            return users; 
            // return BadRequest
            // return Ok
            // return NotFound...
        }

        // We cannot have two matching endpoints because they have the same routes and HTTP methods
        [HttpGet("{id:int}")] // api/users/n
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id); // Attempts to find a user in the database by their ID.

            if (user == null) return NotFound(); // Returns a 404 Not Found response if the user doesn't exist.

            return user; // Returns the user if found.
        }
    }
}
