using Microsoft.AspNetCore.Identity;

namespace Veesy.Domain.Models;

public class MyUser : IdentityUser
{
    public Guid? SubscriptionPlanId { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool VeesyPage { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    public bool SignVeesyContract { get; set; }
    public string LanguageSpoken { get; set; }
    public string ExternalLink { get; set; }
    public string PortfolioIntro { get; set; }
    public string Biografy { get; set; }
    public virtual List<MyUserSector> MyUserSectors { get; set; }
    public virtual List<MyUserUsedSoftware> MyUserUsedSoftwares { get; set; }
    public virtual List<MyUserSkill> MyUserSkills { get; set; }
    public virtual List<Media> Medias { get; set; }
    public virtual List<Portfolio> Portfolios { get; set; }
}