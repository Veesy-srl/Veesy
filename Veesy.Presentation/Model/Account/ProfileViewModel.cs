using Veesy.Domain.Models;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Account;

public class ProfileViewModel
{
    public string Biography { get; set; }
    public string PortfolioIntro { get; set; }
    public List<UsedSoftwareDto> UsedSoftwares { get; set; }
    public List<Guid> SelectedUsedSoftwares { get; set; }
}