namespace Veesy.Domain.Models;

public class Skill : TrackableEntity
{
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual List<MyUserSkill> MyUserSkills { get; set; }
}