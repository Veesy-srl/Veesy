using Veesy.Domain.Constants;

namespace Veesy.Domain.Models;

public class TrackingForm : TrackableEntity
{
    public Guid Id { get; set; }
    public string EmailSender { get; set; }
    public Guid Recipient { get; set; }
    public VeesyConstants.FormType FormType { get; set; }
}