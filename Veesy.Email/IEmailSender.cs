namespace Veesy.Email;

public interface IEmailSender
{
    Task SendEmailAsync(Message message, string templatePATH, List<(string, string)> replacer, List<string> imageFiles = null);
}