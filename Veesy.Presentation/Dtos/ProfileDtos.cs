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
    
}
