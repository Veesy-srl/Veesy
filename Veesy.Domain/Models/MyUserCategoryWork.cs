namespace Veesy.Domain.Models;

public class MyUserCategoryWork
{
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public Guid CategoryWorkId { get; set; }
    public CategoryWork CategoryWork { get; set; }
}