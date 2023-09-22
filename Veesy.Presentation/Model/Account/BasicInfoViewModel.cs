using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Account;

public class BasicInfoViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string Category { get; set; }
    public string VatNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string BasePathImages { get; set; }
    public string FileName { get; set; }
    public List<SectorDto> Sectors { get; set; }
    public string PhoneNumberPrefix { get; set; }
}