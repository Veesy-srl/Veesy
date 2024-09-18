namespace Veesy.Service.Dtos;

public class CreatorFormDto
{
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
    public string Recipient { get; set; }
    public string Message { get; set; }
    public bool Policy { get; set; }
}