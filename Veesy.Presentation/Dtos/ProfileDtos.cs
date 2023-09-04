using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

public class UsedSoftwareDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }

}

public class SkillDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }

}

public class CategoriesWorkDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }

}

public class LanguageSpokenDto
{
    public Guid Code { get; set; }
    public bool Selected { get; set; }
    public string Language { get; set; }
}

public class InfoToShowDto
{
    public Guid Code { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }

}

public static class MapProfileDtos
{    
    
    public static List<UsedSoftwareDto> MapUsedSoftwareList(List<UsedSoftware> usedSoftwares)
    {
        return usedSoftwares.Select(x => new UsedSoftwareDto()
        {
            Code = x.Id,
            Name = x.Name,
            Selected = x.MyUserUsedSoftwares.Count > 0 
        }).ToList();
    }
    
    public static UsedSoftwareDto MapUsedSoftware(UsedSoftware usedSoftware)
    {
        return new UsedSoftwareDto()
        {
            Code = usedSoftware.Id,
            Name = usedSoftware.Name
        };
    }
    
    public static List<SkillDto> MapSkillsList(List<Skill> skills)
    {
        return skills.Select(x => new SkillDto()
        {
            Code = x.Id,
            Name = x.Name,
            Selected = x.MyUserSkills.Count > 0 
        }).ToList();
    }
    
    public static SkillDto MapUsedSoftware(Skill skill)
    {
        return new SkillDto()
        {
            Code = skill.Id,
            Name = skill.Name
        };
    }
    
    public static List<CategoriesWorkDto> MapCategoriesWorkList(List<CategoryWork> categoriesWorks)
    {
        return categoriesWorks.Select(x => new CategoriesWorkDto()
        {
            Code = x.Id,
            Name = x.Name,
            Selected = x.MyUserCategoriesWork.Count > 0 
        }).ToList();
    }
    
    public static CategoriesWorkDto MapCategoriesWork(CategoryWork categoryWork)
    {
        return new CategoriesWorkDto()
        {
            Code = categoryWork.Id,
            Name = categoryWork.Name
        };
    }
    
    public static List<InfoToShowDto> MapInfoToShowList(List<InfoToShow> infoToShows)
    {
        return infoToShows.Select(x => new InfoToShowDto()
        {
            Code = x.Id,
            Name = x.Info,
            Selected = x.MyUserInfoToShows.Count > 0 
        }).ToList();
    }
    
    public static InfoToShowDto MapInfoToShow(InfoToShow categoryWork)
    {
        return new InfoToShowDto()
        {
            Code = categoryWork.Id,
            Name = categoryWork.Info
        };
    }
    
    public static List<LanguageSpokenDto> MapLanguagesSpokenList(List<LanguageSpoken> languagesSpoken)
    {
        return languagesSpoken.Select(x => new LanguageSpokenDto()
        {
            Code = x.Id,
            Language = x.Language,
            Selected = x.MyUserLanguagesSpoken.Count > 0 
        }).ToList();
    }
    
    public static LanguageSpokenDto MapLanguagesSpoken(LanguageSpoken languageSpoken)
    {
        return new LanguageSpokenDto()
        {
            Code = languageSpoken.Id,
            Language = languageSpoken.Language
        };
    }
    
}
