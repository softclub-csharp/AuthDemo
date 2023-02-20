using System.Security.Claims;
using Domain.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(Roles = $"{Roles.Admin}, {Roles.Mentor}, {Roles.Student}")]
public class StudentController : ApiBaseController
{
    
    [HttpGet("UserInfo")]
    public List<ClaimsDto> GetUserInfo()
    {
        return User.Claims.Select(x=>new ClaimsDto{Type = x.Type, Value = x.Value}).ToList();
    }
}