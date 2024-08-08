/*
* Main entry point for application. When we run dotnet command this is where it looks into.
* It's expecting to see a program class. There is an app.Run();
*/
using API.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add and register services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

// A custom extension method that adds identity-related services to the .NET application's dependency injection (DI) container. 
// This method is used to configure and register services required for user authentication, authorization, and identity management.
builder.Services.AddIdentityServices(builder.Configuration);

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

app.UseAuthentication();
app.UseAuthorization();

// Maps the controller endpoints. This method maps HTTP requests to the corresponding
// controller actions, enabling the application to serve API endpoints defined in the controllers.
app.MapControllers();

// Runs the application. This starts the web server and listens for incoming HTTP requests.
app.Run();

