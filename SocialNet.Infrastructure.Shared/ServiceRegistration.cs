using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Domain.Settings;
using SocialNet.Infrastructure.Shared.Services;

namespace SocialNet.Infrastructure.Shared {
  public static class ServiceRegistration {
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
      services.AddTransient<IEmailService, EmailService>();
    }
  }
}