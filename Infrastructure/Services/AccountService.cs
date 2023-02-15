using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AccountService(
        IConfiguration configuration, 
        UserManager<IdentityUser> userManager, 
        DataContext context, IMapper mapper)
    {
       
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
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
        var users = _userManager.Users.ToList();
        return new Response<List<UserDto>>(_mapper.Map<List<UserDto>>(users));
    }


    private async Task<TokenDto> GenerateJWTToken(IdentityUser user)
    {
        return await Task.Run(() =>
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,"Admin")
            };
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
        });

    }
    
    



}