using System;

namespace Veesy.Domain.Models;

public class MediaCategory : TrackableEntity
{
    public Guid MediaId { get; set; }
    public Guid CategoryId { get; set; }
    public Media Media { get; set; }
    public Category Category { get; set; }
}