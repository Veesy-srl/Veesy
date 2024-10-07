using Veesy.Domain.Constants;

namespace Veesy.Domain.Models;

public class TrackingForm : TrackableEntity
{
    public Guid Id { get; set; }
    public string EmailSender { get; set; }
    public string NameSender { get; set; }
    public string? MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public VeesyConstants.FormType FormType { get; set; }
}