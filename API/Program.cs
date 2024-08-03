/*
* Main entry point for application. When we run dotnet command this is where it looks into.
* It's expecting to see a program class. There is an app.Run();
*/

using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add and register services to the container.
builder.Services.AddControllers();
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
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/* We have to add CORS (Cross-origin resource sharing) 
 * Cross-origin resource sharing is a mechanism that allows a web page to access restricted resources 
 * from a server on a domain different than the domain that served the web page
 */
// Adds CORS services to the application, enabling cross-origin requests.
// This is essential when developing client-server applications where the frontend
// and backend are hosted on different domains or ports.
builder.Services.AddCors();

// Builds the application. This method finalizes the setup of the application's
// services and middleware pipeline based on the configured services.
var app = builder.Build();

// Configure the HTTP request pipeline.
// This middleware allows the application to handle CORS requests.
// The specified policy allows requests from any origin specified in WithOrigins,
// permitting any HTTP header and method. This is particularly useful for enabling
// frontend applications (like Angular or React) to communicate with this backend.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));

// Maps the controller endpoints. This method maps HTTP requests to the corresponding
// controller actions, enabling the application to serve API endpoints defined in the controllers.
app.MapControllers();

// Runs the application. This starts the web server and listens for incoming HTTP requests.
app.Run();

