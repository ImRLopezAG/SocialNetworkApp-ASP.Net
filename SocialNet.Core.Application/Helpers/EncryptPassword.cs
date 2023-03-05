using System.Security.Cryptography;
using System.Text;

namespace SocialNet.Core.Application.Helpers;

public static class EncryptPassword {
  public static string Encrypt(string password) {
    using SHA256 hash = SHA256.Create();
    byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
    StringBuilder builder = new();
    foreach (byte b in bytes) {
      builder.Append(b.ToString("x2"));
    }
    return builder.ToString();
  }
}
