using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Endpoint which is our API server is listening on, is detailed here
// Each API Controller has a route, here the name is ["controller"]

[ApiController] // Specifies that this class is an API controller, enabling automatic model validation and other features.
[Route("api/[controller]")] // localhost:5001/api/users and then it is routed to UsersController controller and the endpoints inside it
public class BaseApiController : ControllerBase
{

}
