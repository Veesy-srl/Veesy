using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using ContentDisposition = MimeKit.ContentDisposition;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace Veesy.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;
    public EmailSender(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task SendEmailAsync(Message message, string templatePATH, List<(string, string)> replacer, List<string> imageFiles = null)
    {
        var mailMessage = CreateEmailMessage(message, templatePATH, replacer, imageFiles);
        await SendAsync(mailMessage);
    }
        
   private MimeMessage CreateEmailMessage(Message message, string templatePATH, List<(string, string)> replacer, List<string> imageFiles = null)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("noreply | Veesy", _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = EmailGetBody(templatePATH, replacer)
        };

        string templateDirectory = Path.GetDirectoryName(Path.GetDirectoryName(templatePATH)); // Rimuovi le ultime due cartelle
        string logoPath = Path.Combine(templateDirectory, "imgs", "_Logo Veesy-b.png");

        var logoAttachment = new MimePart("image", "jpeg")
        {
            Content = new MimeContent(File.OpenRead(logoPath), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
            ContentTransferEncoding = ContentEncoding.Base64,
            ContentId = "logo",
            FileName = "_Logo Veesy-b.png"
        };

        builder.LinkedResources.Add(logoAttachment);
        
        if (imageFiles != null)
        {
            foreach (var imageFile in imageFiles)
            {
                string imagePath = Path.Combine(templateDirectory, "imgs", imageFile);
                var imageAttachment = new MimePart("image", "jpeg")
                {
                    Content = new MimeContent(File.OpenRead(imagePath), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline)
                    {
                        FileName = imageFile
                    },
                    ContentTransferEncoding = ContentEncoding.Base64,
                    ContentId = Path.GetFileNameWithoutExtension(imageFile)
                };
                builder.LinkedResources.Add(imageAttachment);
            }
        }
        
        emailMessage.Body = builder.ToMessageBody();
        return emailMessage;
    }
   
    private string EmailGetBody(string templatePATH, List<(string, string)> replacer)
    {
            
        StreamReader str = new(templatePATH);
        string MailText = str.ReadToEnd();
        str.Close();
        foreach (var item in replacer)
            MailText = MailText.Replace(item.Item1, item.Item2);
        return MailText;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }

    private void Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

    public async Task BuildEmail(string emailAddress, string emailMessage)
    {
        
        var message = new Message(new (string, string)[] { ("Noreply | Veesy", emailAddress) }, "Message from user", emailMessage);
        List<(string, string)> replacer = new List<(string, string)> { ("[LinkVerifyMail]", message.Content) };
        var currentPath = Directory.GetCurrentDirectory();
        await SendEmailAsync(message, currentPath + "/wwwroot/MailTemplate/mail-verify-email.html", replacer);
    }
}