using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extentions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        // Add JWT (JSON Web Token) authentication services to the application's dependency injection container.
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Retrieve the token signing key from the configuration settings (e.g., appsettings.json).
                // This key is used to validate the signature of incoming JWT tokens.
                // If the key is not found, an exception is thrown to prevent the application from running with invalid configuration.
                var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not found");

                // Configure the JWT Bearer options to specify how tokens should be validated.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Enable validation of the token's signature using the signing key.
                    // This ensures that the token was issued by a trusted source and has not been tampered with.
                    ValidateIssuerSigningKey = true,

                    // Specify the key that will be used to validate the token's signature.
                    // The key is derived from the tokenKey string, which is encoded as a byte array using UTF-8 encoding.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),

                    // Disable validation of the token's issuer.
                    // The issuer is the entity that issued the token. In some cases, you might want to validate this to ensure the token is from a trusted issuer.
                    // Here, it is set to false, meaning the issuer will not be checked.
                    ValidateIssuer = false,

                    // Disable validation of the token's audience.
                    // The audience is the intended recipient of the token. Like the issuer, you might want to validate this to ensure the token is intended for your application.
                    // Here, it is set to false, meaning the audience will not be checked.
                    ValidateAudience = false
                };
            });

            return services;
    }
}
