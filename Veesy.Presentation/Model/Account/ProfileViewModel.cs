using System.Collections.Generic;
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
    public List<SkillDto> SoftSkills { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string ExternalLink { get; set; }
    public string Role { get; set; }
    public List<MyUserCategoryWork> Category { get; set; }
    public List<RolesWorkDto> RolesWork { get; set; }
    public List<LanguageSpokenDto> LanguagesSpoken { get; set; }
    public List<InfoToShowDto> InfoToShow { get; set; }
    public string FileName { get; set; }
    public string BasePathImages { get; set; }
    public List<PortfolioThumbnailDto> PortfolioThumbnailDtos { get; set; }
    public string BasePath { get; set; }
    public string ApplicationUrl { get; set; }
    public List<SectorDto> Sectors { get; set; }
}