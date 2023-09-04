using Veesy.Domain.Models;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Account;

public class ProfileViewModel
{
    public string Biography { get; set; }
    public string PortfolioIntro { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<UsedSoftwareDto> UsedSoftwares { get; set; }
    public List<Guid> SelectedUsedSoftwares { get; set; }
    public List<SkillDto> HardSkills { get; set; }
    public List<SkillDto> SoftSkills { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string ExternalLink { get; set; }
    public List<CategoriesWorkDto> CategoriesWork { get; set; }
    public List<LanguageSpokenDto> LanguagesSpoken { get; set; }
    public List<InfoToShowDto> InfoToShow { get; set; }
    public string FileName { get; set; }
    public string BasePathImages { get; set; }
}