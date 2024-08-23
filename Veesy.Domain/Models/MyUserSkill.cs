using System;

namespace Veesy.Domain.Models;

public class MyUserSkill : TrackableEntity
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; }
    public char Type { get; set; }
    public bool IsPrincipal { get; set; }
}