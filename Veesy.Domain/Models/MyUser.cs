using Microsoft.AspNetCore.Identity;

namespace Veesy.Domain.Models;

public class MyUser : IdentityUser
{
    public bool VeesyPage { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    public bool SignVeesyContract { get; set; }
    public List<MyUserSector> MyUserSectors { get; set; }
    public List<MyUserUsedSoftware> MyUserUsedSoftwares { get; set; }
}