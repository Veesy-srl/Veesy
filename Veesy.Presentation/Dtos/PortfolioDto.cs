namespace Veesy.Service.Dtos;

public class PortfolioDto
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public bool IsPublic { get; set; }
    public string Password { get; set; }
    public string Link { get; set; }
    public int Status { get; set; }
    public virtual List<PortfolioMediaDto> PortfolioMedias { get; set; }
}