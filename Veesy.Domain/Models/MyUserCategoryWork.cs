namespace Veesy.Domain.Models;

public class MyUserCategoryWork : TrackableEntity
{
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Guid CategoryWorkId { get; set; }
    public CategoryWork CategoryWork { get; set; }
}