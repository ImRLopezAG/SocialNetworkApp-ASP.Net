using SocialNet.Core.Application.DTO;

namespace SocialNet.Core.Application.Interfaces;

public interface IEmailService {
  Task SendEmail(EmailRequest request);
}
