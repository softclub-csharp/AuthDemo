using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController:ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpPost("Register")]
    public async Task<Response<IdentityResult>> Register([FromBody] RegisterDto model)
    {
        return await _accountService.Register(model);
    }
    
    [HttpPost("login")]

    public async Task<Response<TokenDto>> Login([FromBody] LoginDto model)
    {
        return await _accountService.Login(model);
    }
    [HttpGet("Users"),Authorize(Roles = "Admin")]
    public async Task<Response<List<UserDto>>> GetUsers()
    {
        return await _accountService.GetUsers();
    }
}