namespace Veesy.Domain.Models;

public class MyUserLanguageSpoken : TrackableEntity
{
    public string MyUserId { get; set; }
    public Guid LanguageSpokenId { get; set; }
    public MyUser MyUser { get; set; }
    public LanguageSpoken LanguageSpoken { get; set; }
}