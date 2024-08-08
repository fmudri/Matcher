using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    // Register endpoint gives users the ability to register
    [HttpPost("register")] // account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
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

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    // login endpoint
    [HttpPost("login")]
    // Reference to LoginDto.cs
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // We need to compare the username and password with existing users
        /* var user:

    var is a keyword in C# that allows the compiler to infer the type of the variable based on the expression's result.
    In this context, user will be inferred as AppUser? (nullable AppUser), because FirstOrDefaultAsync can return either an instance of AppUser or null.

await:

    await is an asynchronous programming keyword in C# that pauses the execution of the method until the Task being awaited completes.
    In this line, await is used because FirstOrDefaultAsync is an asynchronous method. It allows the method to continue executing asynchronously without blocking the main thread.
    Once the task is completed, the result (either an AppUser instance or null) is assigned to the user variable.

context.Users:

    context refers to an instance of the DataContext class, which typically represents a session with the database in Entity Framework.
    Users is a DbSet<AppUser> property in the DataContext class. It represents the collection of all AppUser entities in the database.

FirstOrDefaultAsync(...):

    FirstOrDefaultAsync is an asynchronous extension method provided by Entity Framework Core.
    It returns the first entity that matches a specified condition (expressed as a lambda function). If no entities match the condition, it returns null.
    The method is asynchronous, meaning it returns a Task<AppUser?>, which is why await is used to retrieve the result.

x => x.UserName == loginDto.Username.ToLower():

    This is a lambda expression used to define the condition that FirstOrDefaultAsync will use to search the Users table.
    x is a parameter representing each AppUser entity in the Users collection during the iteration.
    x.UserName accesses the UserName property of the AppUser instance.
    loginDto.Username.ToLower() converts the Username from the loginDto (likely a Data Transfer Object representing the user's login credentials) to lowercase. This ensures that the comparison is case-insensitive.
    == checks if the UserName of the AppUser entity matches the provided Username in the DTO.
    */

    // Code below is for user authentification
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Invalid username");
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    // Private method to check if a username already exists in the database.
    private async Task<bool> UserExists(string username)
    {
        // Perform a case-insensitive check for the username in the Users DbSet.
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Bob != bob
    }
}
