using System.ComponentModel.DataAnnotations;
using Veesy.Domain.Constants;

namespace Veesy.Domain.Models.Log;

public class UserSecurity
{
    public Guid Id { get; set; } 
    
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
        
    [MaxLength(20)]
    public string? IpAddress { get; set; }
    public DateTime LastAccess { get; set; }
    public string? SessionCode { get; set; }
    
    public string? Browser { get; set; }
    public VeesyConstants.DeviceType DeviceType { get; set; }
    public string? UserAgent { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public int Accuracy { get; set; }
    public string? Timezone { get; set; }
    public int ASN { get; set; }
    public string? Organization { get; set; }
    public string? AreaCode { get; set; }
    public string? OrganizationName { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryCode3 { get; set; }
    public string? ContinentCode { get; set; }
}