using System;

namespace Veesy.Domain.Models;

public class MyUserInfoToShow : TrackableEntity
{
    public string MyUserId { get; set; }
    public Guid InfoToShowId { get; set; }
    public MyUser MyUser { get; set; }
    public InfoToShow InfoToShow { get; set; }
    public bool Show { get; set; }
}