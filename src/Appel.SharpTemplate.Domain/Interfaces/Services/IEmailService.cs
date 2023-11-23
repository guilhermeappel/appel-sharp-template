namespace Appel.SharpTemplate.Domain.Interfaces.Services;

/// <summary>
/// Defines a contract for an email service that supports sending emails and loading email templates.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Asynchronously sends an email.
    /// </summary>
    /// <param name="subject">The subject of the email. Cannot be null, empty, or whitespace.</param>
    /// <param name="message">The message body of the email. Cannot be null, empty, or whitespace.</param>
    /// <param name="email">The recipient's email address. Cannot be null, empty, or whitespace.</param>
    /// <remarks>
    /// Implementations should ensure that the email is sent using appropriate SMTP settings.
    /// Any validation failures (like null or empty parameters) should result in an ArgumentNullException.
    /// Future implementations might include asynchronous processing capabilities to improve performance.
    /// </remarks>
    Task SendAsync(string? subject, string? message, string? email);

    /// <summary>
    /// Asynchronously loads an email template by its name.
    /// </summary>
    /// <param name="templateName">The name of the template to load.</param>
    /// <returns>A string containing the HTML content of the email template.</returns>
    /// <remarks>
    /// Implementations should retrieve the template from a predefined directory and return its content.
    /// This method is intended for loading email templates that can then be customized before sending.
    /// </remarks>
    Task<string> LoadEmailTemplateAsync(string templateName);
}

