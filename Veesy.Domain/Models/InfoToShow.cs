namespace Veesy.Domain.Models;

public class InfoToShow
{
    public Guid Id { get; set; }
    public string Info { get; set; }
    public virtual List<MyUserInfoToShow> MyUserInfoToShows { get; set; }
}