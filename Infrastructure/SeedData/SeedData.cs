using Domain.Constants;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SeedData;

public static  class SeedData
{
    public static void Seed(DataContext context)
    {
        var roles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin),
            new IdentityRole(Roles.Parent),
            new IdentityRole(Roles.User),
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();
    }
    
}