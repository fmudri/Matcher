using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context) : BaseApiController
{
    [HttpPost("register")] // account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        // Check if the username already exists in the database.
        // If it does, return a BadRequest response with an error message.
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        /*
        * We can either not use the using statement and just declare the variable which is using this class,
        * and then at some point, .NET Garbage Collector will come along and clean up any resources
        * that we have not disposed of.

        * A better way to use this is to use 'using' statement. Once this class is out of scope, as int it's
        * not being used anymore, then the dispose method will be called and it will be disposed of.
        */
        using var hmac = new HMACSHA512(); // Creates an instance of HMACSHA512 to generate the password hash and salt.

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(), // Store the username in lowercase to ensure uniqueness.

            // This takes the password user has provided and then, because ComputeHash requires it, converts
            // the string into a byte array. UTF8 is encoding type.
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // Hash the password.

            PasswordSalt = hmac.Key // Store the HMAC key as the password salt.
        };

        // Add the new user to the Users DbSet in the context.
        context.Users.Add(user);
        // Save the changes to the database asynchronously.
        await context.SaveChangesAsync();

        // Return the created user as the response.
        return user;
    }

    // Private method to check if a username already exists in the database.
    private async Task<bool> UserExists(string username)
    {
        // Perform a case-insensitive check for the username in the Users DbSet.
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Bob != bob
    }
}
