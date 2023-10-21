using Microsoft.AspNetCore.Identity;

namespace Veesy.Domain.Models;

public class MyUser : IdentityUser
{
    public Guid? SubscriptionPlanId { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Fullname => Name + " " + Surname;
    public bool VeesyPage { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    public bool SignVeesyContract { get; set; }
    public string LanguageSpoken { get; set; }
    public string ExternalLink { get; set; }
    public string PortfolioIntro { get; set; }
    public string Biografy { get; set; }
    public string Category { get; set; }
    public string VATNumber { get; set; }
    public string OrginalProfileImageName { get; set; }
    public string ProfileImageFileName { get; set; }
    public string PhoneNumberPrefix { get; set; }

    public virtual List<MyUserSector> MyUserSectors { get; set; }
    public virtual List<MyUserUsedSoftware> MyUserUsedSoftwares { get; set; }
    public virtual List<MyUserSkill> MyUserSkills { get; set; }
    public virtual List<Media> Medias { get; set; }
    public virtual List<Portfolio> Portfolios { get; set; }
    public virtual List<MyUserCategoryWork> MyUserCategoriesWork { get; set; }
    public virtual List<MyUserRoleWork> MyUserRolesWork { get; set; }
    public virtual List<MyUserLanguageSpoken> MyUserLanguagesSpoken { get; set; }
    public virtual List<MyUserInfoToShow> MyUserInfosToShow { get; set; }
}