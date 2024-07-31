/*
* Main entry point for application. When we run dotnet command this is where it looks into.
* It's expecting to see a program class. There is an app.Run();
*/

var builder = WebApplication.CreateBuilder(args);

// Add and register services to the container.
builder.Services.AddControllers();

// Builds the application
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
