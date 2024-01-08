namespace Veesy.Domain.Models;

public class RoleWork : TrackableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<MyUserRoleWork> MyUserRolesWork { get; set; }
}