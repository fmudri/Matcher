// Entities folder is the same as "Models".

/* Each class goes into a namespace.
* A namespace is a logical naming structure.
* If I want to call a class AppUser, I can do so but only if it is inside a different namespace.
*
* When Creating a class, typically you would create the namespace as the physical name of the folder
* where you've created a class.
* So, typically, this would be API.Entities as the namespace
*/
namespace API.Entities;

// Inside each class, we have properties
public class AppUser
{
    /*
    * Public is a modifier, it can also be private or internal for example.
    * But, because of Entity framework, it needs to be public. EF works only with public access modifiers.
    * When it comes to EF, the class itself represents a table and each property represents a column.

    * The default value for an int is 0, it can not be null as it is a primitive type.
    * If we were to call it something other than Id, we would need to add [Key] to tell EF that this is
    * an Id because it uses conventions
    */
    public int Id { get; set; }

    /* Because of nullable flag in API.csproj, we have to tell dotnet how we're going to treat certain
    * types of properties in our classes.
    *
    * The default type for a string when nullable is enabled is null, when it is disabled, dotnet is
    * is oblivious to what it is which can cause an exception later on
    */
    public required string UserName { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}
