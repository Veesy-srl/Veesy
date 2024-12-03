namespace Veesy.Domain.Models.Log;

public class UserDeleted
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}