using Domain.Constants;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
// [Authorize(Roles = $"{Roles.Admin},{Roles.Parent}")]
public class AccountController:ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpPost("Register"),AllowAnonymous]
    public async Task<Response<IdentityResult>> Register([FromBody] RegisterDto model)
    {
        return await _accountService.Register(model);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<Response<TokenDto>> Login([FromBody] LoginDto model)
    {
        return await _accountService.Login(model);
    }
    [HttpGet("Users")]
    public async Task<Response<List<UserDto>>> GetUsers()
    {
        return await _accountService.GetUsers();
    }
    [HttpGet("roles")]
    public async Task<Response<List<RoleDto>>> GetRoles()
    {
        return await _accountService.GetRoles();
    }
    
    [HttpPost("assignrole")]
    public async Task<Response<AssignRoleDto>> AssignRole([FromBody] AssignRoleDto role)
    {
        return await _accountService.AssignUserRole(role);
    }
    
    
}