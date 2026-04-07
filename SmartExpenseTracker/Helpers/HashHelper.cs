using System.Security.Cryptography;
using System.Text;

namespace SmartExpenseTracker.Helpers;

public static class HashHelper
{
    /// <summary>
    /// SHA-256 hash of the PIN (hex string).
    /// </summary>
    public static string HashPin(string pin)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        return Convert.ToHexString(bytes);
    }
}
