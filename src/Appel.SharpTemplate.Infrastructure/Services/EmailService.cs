using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Common.Constants;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Appel.SharpTemplate.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendAsync(string? subject, string? message, string? email)
    {
        // @TODO: Add hangfire (or research another option) to prevent the application from hanging while sending the email.

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentNullException(nameof(subject), "Subject cannot be null, empty, or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Message cannot be null, empty, or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null, empty, or whitespace.");
        }

        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(GeneralConstants.Project.NAME, _emailSettings.User));
        mimeMessage.To.Add(new MailboxAddress(string.Empty, email));
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart("html") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailSettings.SmtpHost, int.Parse(_emailSettings.SmtpPort!));
            await client.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }

    public async Task<string> LoadEmailTemplateAsync(string templateName)
    {
        using (var sr = File.OpenText(Path.Combine(Directory.GetCurrentDirectory(), GeneralConstants.FolderNames.EMAIL_TEMPLATES, $"{templateName}.html")))
        {
            return await sr.ReadToEndAsync();
        }
    }
}
