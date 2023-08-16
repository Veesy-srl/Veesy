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
        
    }
}