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
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Builds the application
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
