using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

// The TokenService class implements the ITokenService interface.
// It is responsible for generating JWT (JSON Web Token) tokens for authentication.
public class TokenService(IConfiguration config) : ITokenService
{
    // The CreateToken method generates a JWT token for the given AppUser.
    // The token is used to authenticate and authorize the user in the application.
    public string CreateToken(AppUser user)
    {
        // Retrieve the "TokenKey" from the configuration (e.g., appsettings.json).
        // This key is used to sign the JWT token. If the key is not found, an exception is thrown.
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");

        // Ensure that the tokenKey is sufficiently secure by requiring it to be at least 64 characters long.
        // This is to prevent weak signing keys that could compromise security.
        if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

        // Convert the tokenKey (a string) into a byte array using UTF-8 encoding.
        // This byte array is then used to create a symmetric security key for signing the token.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // Create a list of claims that will be embedded in the JWT token.
        // A claim is a piece of information about the user, such as their username or ID.
        // Here, a single claim is added for the user's NameIdentifier, which is typically their username.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName)
        };

        // Create the signing credentials using the security key and a specific algorithm (HMAC-SHA512).
        // These credentials are used to sign the JWT, ensuring its integrity and authenticity.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        // Create a SecurityTokenDescriptor, which describes the token's properties.
        // - Subject: The claims identity, which contains the claims we want to include in the token.
        // - Expires: The expiration time of the token. Here, it's set to expire in 7 days.
        // - SigningCredentials: The credentials used to sign the token.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token will be valid for 7 days from the time of creation.
            SigningCredentials = creds
        };

        // Create a token handler, which is responsible for creating the JWT token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Create the token using the token descriptor.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Serialize the token to a string and return it.
        // This string represents the JWT and can be sent to the client for use in authentication.
        return tokenHandler.WriteToken(token);
    }
}

