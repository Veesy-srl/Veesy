namespace Veesy.Domain.Models;

public abstract class TrackableEntity : ITrack
{
    public DateTime CreateRecordDate { get; set; }
    public DateTime LastEditRecordDate { get; set; }
    
    //public string IpAddress { get; set; } TODO: Capire come gestire controlli in base a IP o MAC Address;
    public string CreateUserId { get; set; }
    public string LastEditUserId { get; set; }
}

public interface ITrack
{
    public DateTime CreateRecordDate { get; set; }
    public DateTime LastEditRecordDate { get; set; }
    
    //public string IpAddress { get; set; } TODO: Capire come gestire controlli in base a IP o MAC Address;
    public string CreateUserId { get; set; }
    public string LastEditUserId { get; set; }
}