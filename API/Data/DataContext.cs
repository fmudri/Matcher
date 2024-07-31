using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

/*
* We need to derive from a entity framework class called DbContext, hence the ":"
* Around ":" is a constructor. When a new instance of this data context is created, the constructor is executed along with the code inside
*/
public class DataContext(DbContextOptions options) : DbContext(options)
{
    /* A DbSet can be used to query and save instances of TEntity. LINQ queries against a DbSet will be translated into queries against the database.
    * In <> goes type of entity we've created that we want to use for our DB set
    */
    public DbSet<AppUser> Users { get; set; }
}
