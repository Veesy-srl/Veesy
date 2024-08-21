using System.Collections.Generic;
using Veesy.Domain.Models;

namespace Veesy.Presentation.Model.Admin;

public class CreatorViewModel
{
    public List<Domain.Models.Portfolio> Portfolios { get; set; }
    public List<Domain.Models.Media> Medias { get; set; }
    public MyUser User { get; set; }
    public int ProfilePercentCompiled { get; set; }
    public int ElemProfileCompiled { get; set; }
}