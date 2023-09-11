namespace Veesy.Domain.Models;

public class LanguageSpoken : TrackableEntity
{
    public Guid Id { get; set; }
    public string Language { get; set; }
    public virtual List<MyUserLanguageSpoken> MyUserLanguagesSpoken { get; set; }
}