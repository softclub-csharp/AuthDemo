using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
public class JournalController : ApiBaseController
{
    
    [HttpGet("JournalInfo")]
    public List<ClaimsDto> GetJournalInfo()
    {
        return User.Claims.Select(x=>new ClaimsDto{Type = x.Type, Value = x.Value}).ToList();
    }
}