using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountService(
        IConfiguration configuration, 
        UserManager<IdentityUser> userManager, 
        DataContext context, IMapper mapper, RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
    }
    
    public async Task<Response<IdentityResult>> Register(RegisterDto registerDto)
    {
        
        var user = new IdentityUser()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
        };
        var result = await _userManager.CreateAsync(user,registerDto.Password);
        return new Response<IdentityResult>(result);
    }

    public async Task<Response<TokenDto>> Login(LoginDto model)
    {
        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing == null)
            return new Response<TokenDto>(HttpStatusCode.BadRequest, new List<string>(){"Incorrect passsword or login"});

        var check = await _userManager.CheckPasswordAsync(existing, model.Password);
        if (check == true)
        {
            return new Response<TokenDto>(await GenerateJWTToken(existing));
        }
        else 
            return new Response<TokenDto>(HttpStatusCode.BadRequest, new List<string>(){"Incorrect passsword or login"});
        
    }

    public async Task<Response<List<UserDto>>> GetUsers()
    {
        var users = await _context.Users.Select(x=>new UserDto()
        {
            Id = x.Id,
            Username = x.UserName,
            PhoneNumber = x.PhoneNumber,
            Roles = (from ur in _context.UserRoles
                    join r in _context.Roles on ur.RoleId equals r.Id 
                    where x.Id == ur.UserId select new RoleDto()
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).ToList()
        }).ToListAsync();
        
        return new Response<List<UserDto>>(_mapper.Map<List<UserDto>>(users));
    }
    
    public async Task<Response<List<RoleDto>>> GetRoles()
    {
        var roles = _context.Roles.ToList();
        return new Response<List<RoleDto>>(_mapper.Map<List<RoleDto>>(roles));
    }
    
    public async Task<Response<RoleDto>> AddRole(RoleDto model)
    {
        var role = new IdentityRole(model.Name);
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return new Response<RoleDto>(HttpStatusCode.OK, new List<string>() { "Role created successfully" });
        }
        else 
            return new Response<RoleDto>(HttpStatusCode.BadRequest, new List<string>(){"Role creation failed"});
    }

    public async Task<Response<AssignRoleDto>> AssignUserRole(AssignRoleDto model)
    {
        try
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x=>x.Name.ToUpper() == model.RoleName.ToUpper());
            var user = await _context.Users.FindAsync(model.UserId);
            await _userManager.AddToRoleAsync(user, role.Name);
            return new Response<AssignRoleDto>(model);
        }
        catch (Exception ex)
        {
            return new Response<AssignRoleDto>(HttpStatusCode.InternalServerError, new List<string>() { ex.Message });
        }
    }

    private async Task<TokenDto> GenerateJWTToken(IdentityUser user)
    {
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            
            //get all roles belonging to the user
            var roles = await _userManager.GetRolesAsync(user);
            //add all roles into claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            //fill token 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return new TokenDto(tokenString);
    

    }
    
}