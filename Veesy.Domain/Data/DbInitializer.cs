using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                Email = "lorenzo.vettori11@gmail.com",
                EmailConfirmed = true
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
                Email = "lorenzo.vettori@enigma-tech.it",
                EmailConfirmed = true
            };

            IdentityResult result = userManager.CreateAsync(adm2, "Antani123!").Result;
            var addedRole = userManager.AddToRolesAsync(adm2, new[] { Roles.User }).Result;

        }
        
        var subscriptionPlans = new List<SubscriptionPlan>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Free", Description = "Free", Price = 0.0m, AllowedMediaNumber = 10, AllowedMegaByte = 100, IsMediaFormatsInclude = false},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Pro", Description = "Pro", Price = 1.0m, AllowedMediaNumber = 5, AllowedMegaByte = 5000, IsMediaFormatsInclude = false},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Plus", Description = "Plus", Price = 2.0m, AllowedMediaNumber = -1, AllowedMegaByte = 10000, IsMediaFormatsInclude = false},
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
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software A", Description = "Software A"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software B", Description = "Software B"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software C", Description = "Software C"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software D", Description = "Software D"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software E", Description = "Software E"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software F", Description = "Software F"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software G", Description = "Software G"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software H", Description = "Software H"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software I", Description = "Software I"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software J", Description = "Software J"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software K", Description = "Software K"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software L", Description = "Software L"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software M", Description = "Software M"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software N", Description = "Software N"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Software O", Description = "Software O"},
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
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore A", Description = "Settore A"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore B", Description = "Settore B"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore C", Description = "Settore C"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore D", Description = "Settore D"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore E", Description = "Settore E"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Settore F", Description = "Settore F"},
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
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill A"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill B"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill C"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill D"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill E"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill F"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill G"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill H"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill I"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill J"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill K"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Skill L"},
        };
        var dbSkills = dbContext.Skills.ToList();
        foreach (var item in skills)
        {
            if (!dbSkills.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var categoriesWork = new List<CategoryWork>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category A", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category B", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category C", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category D", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category E", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category F", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category G", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category H", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category I", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category J", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category K", Description = "Category Work"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Work Category L", Description = "Category Work"},
        };
        var dbCategoyWork = dbContext.CategoriesWork.ToList();
        foreach (var item in categoriesWork)
        {
            if (!dbCategoyWork.Any(x => x.Name == item.Name))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var languages = new List<LanguageSpoken>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Afrikaans"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Albanian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Arabic"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Armenian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Basque"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Bengali"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Bulgarian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Catalan"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Cambodian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Chinese (Mandarin)"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Croatian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Czech"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Danish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Dutch"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "English"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Estonian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Fiji"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Finnish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "French"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Georgian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "German"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Greek"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Gujarati"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Hebrew"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Hindi"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Hungarian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Icelandic"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Indonesian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Irish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Italian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Japanese"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Javanese"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Korean"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Latin"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Latvian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Lithuanian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Macedonian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Malay"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Malayalam"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Maltese"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Maori"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Marathi"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Mongolian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Nepali"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Norwegian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Persian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Polish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Portuguese"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Punjabi"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Quechua"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Romanian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Russian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Samoan"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Serbian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Slovak"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Slovenian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Spanish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Swahili"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Swedish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Tamil"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Tatar"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Telugu"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Thai"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Tibetan"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Tonga"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Turkish"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Ukrainian"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Urdu"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Uzbek"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Vietnamese"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Welsh"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Language = "Xhosa"},
        };
        var dbLanguage = dbContext.LanguagesSpoken.ToList();
        foreach (var item in languages)
        {
            if (!dbLanguage.Any(x => x.Language == item.Language))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();

        var infosToShow = new List<InfoToShow>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Info = "Software"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Info = "Soft Skills"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Info = "Hard Skill"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Info = "Downloadable CV"},
            
        };
        var dbInfoToShow = dbContext.InfosToShow.ToList();
        foreach (var item in infosToShow)
        {
            if (!dbInfoToShow.Any(x => x.Info == item.Info))
                dbContext.Add(item);
        }
        dbContext.SaveChanges();
        
        var mediaFormats = new List<Format>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Description = "2000", Type = "Image", Width = 2000, Name = "Full HD"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Description = "1280", Type = "Image", Width = 1280, Name = "HD"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Description = "1920", Type = "Video", Width = 1920, Name = "Full HD"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Description = "1280", Type = "Video", Width = 1280, Name = "HD"},
            
        };
        var dbFormats = dbContext.Formats.ToList();
        foreach (var item in mediaFormats)
        {
            if (!dbFormats.Any(x => x.Name == item.Name && x.Type == item.Type))
                dbContext.Formats.Add(item);
        }
        dbContext.SaveChanges();
            
#if DEBUG
        var users = dbContext.MyUsers
            .Include(s => s.MyUserInfosToShow)
            .ThenInclude(s => s.InfoToShow)
            .ToList();
        infosToShow = dbContext.InfosToShow.ToList();
        foreach (var user in users)
        {
            if (user.MyUserInfosToShow.Count == 0)
            {
                user.MyUserInfosToShow = new List<MyUserInfoToShow>();
                foreach (var info in infosToShow)
                {
                    user.MyUserInfosToShow.Add(new MyUserInfoToShow()
                    {
                        InfoToShowId = info.Id,
                        Show = true,
                        LastEditUserId = "init",
                        CreateUserId = "init",
                    });
                }
            }
            else
            {
                foreach (var info in infosToShow)
                {
                    if(!user.MyUserInfosToShow.Any(s => s.InfoToShowId == info.Id)){
                        user.MyUserInfosToShow.Add(new MyUserInfoToShow()
                        {
                            InfoToShowId = info.Id,
                            Show = true,
                            LastEditUserId = "init",
                            CreateUserId = "init",
                        });
                    }
                }
            }
        }
        dbContext.MyUsers.UpdateRange(users);
        dbContext.SaveChanges();
#endif

    }
}