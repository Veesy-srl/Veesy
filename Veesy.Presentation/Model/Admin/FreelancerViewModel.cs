using Veesy.Domain.Models;

namespace Veesy.Presentation.Model.Admin;

public class FreelancerViewModel
{
    public List<Domain.Models.Portfolio> Portfolios { get; set; }
    public List<Domain.Models.Media> Medias { get; set; }
    public MyUser User { get; set; }
}