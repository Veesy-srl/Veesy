namespace Veesy.Domain.Models;

public class MyUserRoleWork : TrackableEntity
{
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Guid RoleWorkId { get; set; }
    public RoleWork RoleWork { get; set; }
}