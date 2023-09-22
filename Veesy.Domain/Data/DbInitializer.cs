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
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Beta", Description = "Beta", Price = 0.0m, AllowedMediaNumber = 100, AllowedMegaByte = 250, IsMediaFormatsInclude = false},
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
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Substance Painter", Description = "Adobe Substance Painter"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Blender", Description = "Blender"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Blender Cycles", Description = "Blender Cycles"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Blender Evee", Description = "Blender Evee"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "C++", Description = "C++"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "CSS", Description = "CSS"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Django Framework", Description = "Django Framework"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "HTML", Description = "HTML"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Linux (System Administration)", Description = "Linux (System Administration)"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Python", Description = "Python"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Unreal Engine", Description = "Unreal Engine"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe After Effects", Description = "Adobe After Effects"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Audition", Description = "Adobe Audition"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Illustrator", Description = "Adobe Illustrator"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Indesign", Description = "Adobe Indesign"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Lightroom", Description = "Adobe Lightroom"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Photoshop", Description = "Adobe Photoshop"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Premiere", Description = "Adobe Premiere"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe XD", Description = "Adobe XD"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Autodesk 3ds Max", Description = "Autodesk 3ds Max"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Blackmagic Da Vinci Resolve", Description = "Blackmagic Da Vinci Resolve"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Maxon Cinema4d", Description = "Maxon Cinema4d"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Maxon Redshift", Description = "Maxon Redshift"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Autodesk Maya", Description = "Autodesk Maya"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "ZBrush", Description = "ZBrush"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Webflow", Description = "Webflow"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Wordpress", Description = "Wordpress"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Chaos Corona Render", Description = "Chaos Corona Render"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Figma", Description = "Figma"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Keyshot", Description = "Keyshot"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "McNeil Rhinoceros", Description = "McNeil Rhinoceros"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Chaos Vray", Description = "Chaos Vray"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Forest Pack", Description = "Forest Pack"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Marvelous Designer", Description = "Marvelous Designer"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Marmoset", Description = "Marmoset"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adobe Substance Designer", Description = "Adobe Substance Designer"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Unity", Description = "Unity"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Railclone", Description = "Railclone"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Sketchup", Description = "Sketchup"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Quixel Mixer", Description = "Quixel Mixer"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Autodesk Fusion 360", Description = "Autodesk Fusion 360"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Autodesk Revit", Description = "Autodesk Revit"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Krita", Description = "Krita"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Procreate", Description = "Procreate"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Rizom UV", Description = "Rizom UV"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Speedtree", Description = "Speedtree"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Aseprite", Description = "Aseprite"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Spine Pro", Description = "Spine Pro"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Gaea", Description = "Gaea"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Daz Studio", Description = "Daz Studio"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Otoy Octane", Description = "Otoy Octane"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "FStorm Render", Description = "FStorm Render"},
        }; 
        
        var dbSoftwares = dbContext.UsedSoftwares.ToList();
        foreach (var item in usedSoftwares)
        {
            if (!dbSoftwares.Any(x => x.Name == item.Name))
            {
                dbContext.Add(item);
                dbSoftwares.Add(item);
            }
        }
        dbContext.SaveChanges();
        
        var sectors = new List<Sector>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Automotive", Description = "Automotive"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Metaverso", Description = "Metaverso"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Prodotto", Description = "Prodotto"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "VR / AR / XR", Description = "VR / AR / XR"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "CGI", Description = "CGI"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Game Art", Description = "Game Art"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Archiviz", Description = "Archiviz"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Interior Design", Description = "Interior Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Light Design", Description = "Light Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Graphic Design", Description = "Graphic Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "UX/UI", Description = "UX/UI"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Character", Description = "Character"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Grafica", Description = "Grafica"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Concept Art", Description = "Concept Art"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Illustrazione", Description = "Illustrazione"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Brand Identity", Description = "Brand Identity"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Logo Design", Description = "Logo Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Visual Design", Description = "Visual Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Arte Digitale", Description = "Arte Digitale"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Environnement Design", Description = "Environnement Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fumetto", Description = "Fumetto"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Motion Design", Description = "Motion Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Storyboarding", Description = "Storyboarding"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Video Making", Description = "Video Making"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "VFX", Description = "VFX"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Motion Graphic", Description = "Motion Graphic"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Architecture", Description = "Architecture"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Design Navale", Description = "Design Navale"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "3D Visualization", Description = "3D Visualization"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fashion", Description = "Fashion"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Exibithion Design", Description = "Exibithion Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Cover Design", Description = "Cover Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Multimedia Design", Description = "Multimedia Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Art Direction", Description = "Art Direction"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Real Time Games", Description = "Real Time Games"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fotografia", Description = "Fotografia"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Illustrazione", Description = "Illustrazione"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fashion", Description = "Fashion"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Grafica d'arte", Description = "Grafica d'arte"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Storyboarding", Description = "Storyboarding"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Game Development", Description = "Game Development"},
        };
        var dbSectors = dbContext.Sectors.ToList();
        foreach (var item in sectors)
        {
            if (!dbSectors.Any(x => x.Name == item.Name))
            {
                dbContext.Add(item);
                dbSectors.Add(item);
            }
        }
        dbContext.SaveChanges();
        
        var skills = new List<Skill>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Creativity"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Communication"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adaptability"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Time Management"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Attention to Detail"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Problem Solving"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Critical Thinking"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Teamwork"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Patience"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Empathy"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Open-mindedness"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adaptability to Technology"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Networking"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Organization"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Cultural Awareness"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Presentation Skills"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Negotiation"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Marketing"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Client Management"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Self-Motivation"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Adherence to Deadlines"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Storytelling"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Conflict Resolution"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Resourcefulness"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Cultural Sensitivity"},
        };
        var dbSkills = dbContext.Skills.ToList();
        foreach (var item in skills)
        {
            if (!dbSkills.Any(x => x.Name == item.Name))
            {
                dbContext.Add(item);
                dbSkills.Add(item);
            }
        }
        dbContext.SaveChanges();
        
        var categoriesWork = new List<CategoryWork>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "3D", Description = "3D"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "2D", Description = "2D"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Web Design / UX / UI", Description = "Web Design / UX / UI"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "VR / AR / XR / Metaverse", Description = "VR / AR / XR / Metaverse"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Photography", Description = "Photography"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Illustration", Description = "Illustration"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Motion Design / Graphic", Description = "Motion Design / Graphic"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Sound Design", Description = "Sound Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Video / Film Making", Description = "Video / Film Making"},
        };
        var dbCategoyWork = dbContext.CategoriesWork.ToList();
        foreach (var item in categoriesWork)
        {
            if (!dbCategoyWork.Any(x => x.Name == item.Name))
            {
                dbContext.Add(item);
                dbCategoyWork.Add(item);
            }
        }
        dbContext.SaveChanges();
        
        var rolesWork = new List<RoleWork>()
        {
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Automotive", Description = "Automotive"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Metaverso", Description = "Metaverso"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Prodotto", Description = "Prodotto"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "VR / AR / XR", Description = "VR / AR / XR"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "CGI", Description = "CGI"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Game Art", Description = "Game Art"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Archiviz", Description = "Archiviz"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Interior Design", Description = "Interior Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Light Design", Description = "Light Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Graphic Design", Description = "Graphic Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "UX/UI", Description = "UX/UI"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Character", Description = "Character"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Grafica", Description = "Grafica"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Concept Art", Description = "Concept Art"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Illustrazione", Description = "Illustrazione"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Brand Identity", Description = "Brand Identity"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Logo Design", Description = "Logo Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Visual Design", Description = "Visual Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Arte Digitale", Description = "Arte Digitale"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Environnement Design", Description = "Environnement Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fumetto", Description = "Fumetto"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Motion Design", Description = "Motion Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Storyboarding", Description = "Storyboarding"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Video Making", Description = "Video Making"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "VFX", Description = "VFX"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Motion Graphic", Description = "Motion Graphic"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Architecture", Description = "Architecture"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Design Navale", Description = "Design Navale"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "3D Visualization", Description = "3D Visualization"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fashion", Description = "Fashion"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Exibithion Design", Description = "Exibithion Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Cover Design", Description = "Cover Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Multimedia Design", Description = "Multimedia Design"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Art Direction", Description = "Art Direction"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Real Time Games", Description = "Real Time Games"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fotografia", Description = "Fotografia"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Illustrazione", Description = "Illustrazione"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Fashion", Description = "Fashion"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Grafica d'arte", Description = "Grafica d'arte"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Storyboarding", Description = "Storyboarding"},
            new (){LastEditUserId = "Init", CreateUserId = "init", Name = "Game Development", Description = "Game Development"}
        };
        var dbRoleWorks= dbContext.RolesWork.ToList();
        foreach (var item in rolesWork)
        {
            if (!dbRoleWorks.Any(x => x.Name == item.Name))
            {
                dbContext.Add(item);
                dbRoleWorks.Add(item);
            }
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
            {
                dbContext.Add(item);
                dbLanguage.Add(item);
            }
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
            {
                dbContext.Add(item);
                dbInfoToShow.Add(item);
            }
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