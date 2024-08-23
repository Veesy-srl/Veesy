using System;
using Veesy.Domain.Constants;

namespace Veesy.Domain.Models;

public class ReferralLinkTracking
{
    public Guid Id { get; set; }
    public Guid? ReferralLinkId { get; set; }
    public ReferralLink? ReferralLink { get; set; }
    public string? Ip { get; set; }
    public string? Referer { get; set; }
    public DateTime LastAccess { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Browser { get; set; }
    public VeesyConstants.DeviceType DeviceType { get; set; }
    public string? UserAgent { get; set; }
}