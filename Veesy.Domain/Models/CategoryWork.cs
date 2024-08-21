using System;
using System.Collections.Generic;

namespace Veesy.Domain.Models;

public class CategoryWork : TrackableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<MyUserCategoryWork> MyUserCategoriesWork { get; set; }
}