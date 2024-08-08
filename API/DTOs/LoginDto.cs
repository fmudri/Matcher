/* A DTO (Data Transfer Object) is a simple object used to transfer data between different parts of an application, 
* such as between the client and the server or between different layers of the application 
* (e.g., from the database layer to the service layer). DTOs are primarily used to encapsulate the data and send it across the network, 
* or between processes, without exposing the underlying business logic or the full details of the entities used within the application.
 */
 
using System;

namespace API.DTOs;

public class LoginDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
