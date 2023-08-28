using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;

namespace Veesy.Domain.Data;

public class DbInitializer
{
    public static void SeedUsersAndRoles(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager,
        ApplicationDbContext dbContext, IConfiguration config)
    {
        string[] roleNames = { Roles.User, Roles.Admin, Roles.Developer};

        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).Result;
            if (!roleExist)
                roleResult = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        }

        var adm1 = userManager.FindByEmailAsync("lorenzo.vettori11@gmail.com").Result;
        if (adm1 == null)
        {
            adm1 = new MyUser
            {
                UserName = "lore_vetto11",
                Email = "lorenzo.vettori11@gmail.com"
            };

            IdentityResult result = userManager.CreateAsync(adm1, "Antani123!").Result;
            var addedRole = userManager.AddToRolesAsync(adm1, new[] { Roles.Admin}).Result;
        }

        var adm2 = userManager.FindByEmailAsync("lorenzo.vettori@enigma-tech.it").Result;
        if (adm2 == null)
        {
            adm2 = new MyUser
            {
                UserName = "lv_enigma",
                Email = "lorenzo.vettori@enigma-tech.it"
            };

            IdentityResult result = userManager.CreateAsync(adm2, "Antani123!").Result;
            var addedRole = userManager.AddToRolesAsync(adm2, new[] { Roles.User }).Result;

        }
        
        var subscriptionPlans = new List<SubscriptionPlan>()
        {
            new (){Name = "Free", Description = "Free", Price = 0.0m, AllowedMediaNumber = 10, AllowedMegaByte = 100, IsMediaFormatsInclude = false},
            new (){Name = "Pro", Description = "Pro", Price = 1.0m, AllowedMediaNumber = 5, AllowedMegaByte = 5000, IsMediaFormatsInclude = false},
            new (){Name = "Plus", Description = "Plus", Price = 2.0m, AllowedMediaNumber = -1, AllowedMegaByte = 10000, IsMediaFormatsInclude = false},
        };
        var dbSubscriptions = dbContext.SubscriptionPlans.ToList();
        foreach (var item in subscriptionPlans)
        {
            if (!dbSubscriptions.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var usedSoftwares = new List<UsedSoftware>()
        {
            new (){Name = "Software A", Description = "Software A"},
            new (){Name = "Software B", Description = "Software B"},
            new (){Name = "Software C", Description = "Software C"},
            new (){Name = "Software D", Description = "Software D"},
            new (){Name = "Software E", Description = "Software E"},
            new (){Name = "Software F", Description = "Software F"},
            new (){Name = "Software G", Description = "Software G"},
            new (){Name = "Software H", Description = "Software H"},
            new (){Name = "Software I", Description = "Software I"},
            new (){Name = "Software J", Description = "Software J"},
            new (){Name = "Software K", Description = "Software K"},
            new (){Name = "Software L", Description = "Software L"},
            new (){Name = "Software M", Description = "Software M"},
            new (){Name = "Software N", Description = "Software N"},
            new (){Name = "Software O", Description = "Software O"},
        }; 
        
        var dbSoftwares = dbContext.UsedSoftwares.ToList();
        foreach (var item in usedSoftwares)
        {
            if (!dbSoftwares.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var sectors = new List<Sector>()
        {
            new (){Name = "Settore A", Description = "Settore A"},
            new (){Name = "Settore B", Description = "Settore B"},
            new (){Name = "Settore C", Description = "Settore C"},
            new (){Name = "Settore D", Description = "Settore D"},
            new (){Name = "Settore E", Description = "Settore E"},
            new (){Name = "Settore F", Description = "Settore F"},
        };
        var dbSectors = dbContext.Sectors.ToList();
        foreach (var item in sectors)
        {
            if (!dbSectors.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var skills = new List<Skill>()
        {
            new (){Name = "Skill A"},
            new (){Name = "Skill B"},
            new (){Name = "Skill C"},
            new (){Name = "Skill D"},
            new (){Name = "Skill E"},
            new (){Name = "Skill F"},
            new (){Name = "Skill G"},
            new (){Name = "Skill H"},
            new (){Name = "Skill I"},
            new (){Name = "Skill J"},
            new (){Name = "Skill K"},
            new (){Name = "Skill L"},
        };
        var dbSkills = dbContext.Skills.ToList();
        foreach (var item in skills)
        {
            if (!dbSkills.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();


    }
}