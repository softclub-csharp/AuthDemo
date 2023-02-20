using Domain.Constants;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize(Roles = $"{Roles.Admin},{Roles.Mentor}")]
public class MentorController : ApiBaseController
{
    
    [HttpGet("MentorInfo")]
    public List<ClaimsDto> GetUserInfo()
    {
        return User.Claims.Select(x=>new ClaimsDto{Type = x.Type, Value = x.Value}).ToList();
    }
}