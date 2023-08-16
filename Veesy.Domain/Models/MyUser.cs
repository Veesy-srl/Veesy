using Microsoft.AspNetCore.Identity;

namespace Veesy.Domain.Models;

public class MyUser : IdentityUser
{
    public Guid? SubscriptionPlanId { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public bool VeesyPage { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    public bool SignVeesyContract { get; set; }
    public virtual List<MyUserSector> MyUserSectors { get; set; }
    public virtual List<MyUserUsedSoftware> MyUserUsedSoftwares { get; set; }
    public virtual List<Media> Medias { get; set; }
    public virtual List<Portfolio> Portfolios { get; set; }
}