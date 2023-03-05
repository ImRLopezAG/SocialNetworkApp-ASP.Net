namespace SocialNet.Core.Application.Core;

public class ServiceResult {
  public ServiceResult() => Success = true;
  public bool Success { get; set; }
  public string? Message { get; set; }
  public object? Data { get; set; }
}
