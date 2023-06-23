namespace Veesy.Domain.Models;

public class CommonInfo
{
    public DateTime CreateRecordDate { get; set; }
    public DateTime LastEditRecordDate { get; set; }
    public string IpAddress { get; set; }
    public string CreateUserId { get; set; }
    public string LastEditUserId { get; set; }
}