using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SocialNet.Core.Application.DTO;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Domain.Settings;

namespace SocialNet.Infrastructure.Shared.Services;

public class EmailService : IEmailService {
  private readonly MailSettings _mailSettings;
  public EmailService(IOptions<MailSettings> mailSettings) => _mailSettings = mailSettings.Value;
  public async Task SendEmail(EmailRequest request) {
    try {
      MimeMessage message = new() {
        Sender = MailboxAddress.Parse($"{_mailSettings.DisplayName} <{_mailSettings.EmailFrom}>")
      };
      message.To.Add(MailboxAddress.Parse(request.To));
      message.Subject = request.Subject;
      BodyBuilder builder = new() {
        HtmlBody = request.Body
      };
      message.Body = builder.ToMessageBody();

      using SmtpClient smtp = new();
      smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
      smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
      await smtp.SendAsync(message);
      smtp.Disconnect(true);
    } catch {
      throw;
    }
  }
}
