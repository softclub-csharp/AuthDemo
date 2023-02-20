using Domain.Constants;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.SeedData;

public static  class SeedData
{
    public static void Seed(DataContext context)
    {
        if(context.Countries.Any()) return;
        var region = new Region()
        {
            Id = 1,
            Name = "Asia"
        };
        context.Regions.Add(region);
        var countries = new List<Country>()
        {
            new Country(){Id = 1,Name = "Tajikistan",RegionId = 1},
            new Country(){Id = 2,Name = "Russia",RegionId = 1},
            new Country(){Id = 3,Name = "USA",RegionId = 1},
            //generate 4000 countries
            new Country(){Id = 4001,Name = "Country 4001",RegionId = 1},
            new Country(){Id = 4002,Name = "Country 4002",RegionId = 1},
            
        };
        for(int i=4; i<=300; i++){
            countries.Add(new Country(){Id = i,Name = $"Country {i}",RegionId = 1});
        }
        
        context.Countries.AddRange(countries);
        context.SaveChanges();
        
        if(context.Roles.Any()) return;
        
        var roles = new List<IdentityRole>()
        {
            new IdentityRole(Roles.Admin){NormalizedName = Roles.Admin.ToUpper()},
            new IdentityRole(Roles.Parent){NormalizedName = Roles.Parent.ToUpper()},
            new IdentityRole(Roles.Mentor){NormalizedName = Roles.Mentor.ToUpper()},
            new IdentityRole(Roles.Student){NormalizedName = Roles.Student.ToUpper()},
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();
    }
    
}