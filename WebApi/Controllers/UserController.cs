using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Authorize(Roles = Roles.User)]
public class UserController:ApiBaseController
{
    [HttpGet("mybooks")]
    public List<string> Get()
    {
        return new List<string>() { "Book1", "Book2" };
    }
}