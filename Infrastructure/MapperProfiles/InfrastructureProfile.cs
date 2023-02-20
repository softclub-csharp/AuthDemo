using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.MapperProfiles;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<IdentityUser, UserDto>();
        CreateMap<IdentityRole, RoleDto>();
        CreateMap<Country, CountryDto>();
    }
}