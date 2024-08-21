using System;

namespace Veesy.Domain.Models;

public class MyUserUsedSoftware : TrackableEntity
{
    public string MyUserId { get; set; }
    public Guid UsedSoftwareId { get; set; }
    public MyUser MyUser { get; set; }
    public UsedSoftware UsedSoftware { get; set; }
    public bool IsPrincipal { get; set; }
}