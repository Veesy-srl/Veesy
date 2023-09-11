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
        
        var categoriesWork = new List<CategoryWork>()
        {
            new (){Name = "Work Category A", Description = "Category Work"},
            new (){Name = "Work Category B", Description = "Category Work"},
            new (){Name = "Work Category C", Description = "Category Work"},
            new (){Name = "Work Category D", Description = "Category Work"},
            new (){Name = "Work Category E", Description = "Category Work"},
            new (){Name = "Work Category F", Description = "Category Work"},
            new (){Name = "Work Category G", Description = "Category Work"},
            new (){Name = "Work Category H", Description = "Category Work"},
            new (){Name = "Work Category I", Description = "Category Work"},
            new (){Name = "Work Category J", Description = "Category Work"},
            new (){Name = "Work Category K", Description = "Category Work"},
            new (){Name = "Work Category L", Description = "Category Work"},
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
            new (){Language = "Afrikaans"},
            new (){Language = "Albanian"},
            new (){Language = "Arabic"},
            new (){Language = "Armenian"},
            new (){Language = "Basque"},
            new (){Language = "Bengali"},
            new (){Language = "Bulgarian"},
            new (){Language = "Catalan"},
            new (){Language = "Cambodian"},
            new (){Language = "Chinese (Mandarin)"},
            new (){Language = "Croatian"},
            new (){Language = "Czech"},
            new (){Language = "Danish"},
            new (){Language = "Dutch"},
            new (){Language = "English"},
            new (){Language = "Estonian"},
            new (){Language = "Fiji"},
            new (){Language = "Finnish"},
            new (){Language = "French"},
            new (){Language = "Georgian"},
            new (){Language = "German"},
            new (){Language = "Greek"},
            new (){Language = "Gujarati"},
            new (){Language = "Hebrew"},
            new (){Language = "Hindi"},
            new (){Language = "Hungarian"},
            new (){Language = "Icelandic"},
            new (){Language = "Indonesian"},
            new (){Language = "Irish"},
            new (){Language = "Italian"},
            new (){Language = "Japanese"},
            new (){Language = "Javanese"},
            new (){Language = "Korean"},
            new (){Language = "Latin"},
            new (){Language = "Latvian"},
            new (){Language = "Lithuanian"},
            new (){Language = "Macedonian"},
            new (){Language = "Malay"},
            new (){Language = "Malayalam"},
            new (){Language = "Maltese"},
            new (){Language = "Maori"},
            new (){Language = "Marathi"},
            new (){Language = "Mongolian"},
            new (){Language = "Nepali"},
            new (){Language = "Norwegian"},
            new (){Language = "Persian"},
            new (){Language = "Polish"},
            new (){Language = "Portuguese"},
            new (){Language = "Punjabi"},
            new (){Language = "Quechua"},
            new (){Language = "Romanian"},
            new (){Language = "Russian"},
            new (){Language = "Samoan"},
            new (){Language = "Serbian"},
            new (){Language = "Slovak"},
            new (){Language = "Slovenian"},
            new (){Language = "Spanish"},
            new (){Language = "Swahili"},
            new (){Language = "Swedish"},
            new (){Language = "Tamil"},
            new (){Language = "Tatar"},
            new (){Language = "Telugu"},
            new (){Language = "Thai"},
            new (){Language = "Tibetan"},
            new (){Language = "Tonga"},
            new (){Language = "Turkish"},
            new (){Language = "Ukrainian"},
            new (){Language = "Urdu"},
            new (){Language = "Uzbek"},
            new (){Language = "Vietnamese"},
            new (){Language = "Welsh"},
            new (){Language = "Xhosa"},
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
            new (){Info = "Software"},
            new (){Info = "Soft Skills"},
            new (){Info = "Hard Skill"},
            new (){Info = "Downloadable CV"},
            
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
            new (){Description = "2000", Type = "Image", Width = 2000, Name = "Full HD"},
            new (){Description = "1280", Type = "Image", Width = 1280, Name = "HD"},
            new (){Description = "1920", Type = "Video", Width = 1920, Name = "Full HD"},
            new (){Description = "1280", Type = "Video", Width = 1280, Name = "HD"},
            
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
                        Show = true
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
                            Show = true
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