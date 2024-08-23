using System;
using System.Collections.Generic;

namespace Veesy.Domain.Models;

public class InfoToShow : TrackableEntity
{
    public Guid Id { get; set; }
    public string Info { get; set; }
    public virtual List<MyUserInfoToShow> MyUserInfoToShows { get; set; }
}