namespace Veesy.Domain.Models;

public class MyUserSector : TrackableEntity
{
    public Guid SectorId { get; set; }
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Sector Sector { get; set; }
    public string Note { get; set; }
    public bool IsPrincipal { get; set; }
}