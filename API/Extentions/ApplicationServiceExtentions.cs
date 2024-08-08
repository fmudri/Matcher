using System;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;

// Static allows us to use method in this class without needing to create a new instance of this class
public static class ApplicationServiceExtentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add and register services to the container.
        services.AddControllers();
        /* This tells our app about the DbContext from DataContext.cs and creates a ConnectionString
        * <NameOfDbContextCreated>
        * When we add a service or register a service in this way that when we use it and the way that
        * and the way we're going to use this is via dependency injection and through dependency injection,
        * then .Net is going to create a new instance of our DataContext class.
        * And via the constructor, it's going to get past the options that we specify here when we add
        * or register this service.
        * To get something out of our configuration, we use builder.
        */
        // Adding the DataContext as a service in the dependency injection container
        // using the AddDbContext method. This sets up the Entity Framework Core context
        // with the specified options, in this case, using SQLite as the database provider.
        // The connection string "DefaultConnection" is retrieved from the configuration file.
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        /* We have to add CORS (Cross-origin resource sharing) 
         * Cross-origin resource sharing is a mechanism that allows a web page to access restricted resources 
         * from a server on a domain different than the domain that served the web page
         */
        // Adds CORS services to the application, enabling cross-origin requests.
        // This is essential when developing client-server applications where the frontend
        // and backend are hosted on different domains or ports.
        services.AddCors();

        // Scoped services are create once per client request
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
