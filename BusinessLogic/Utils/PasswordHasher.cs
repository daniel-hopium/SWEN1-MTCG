using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Utils;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public static bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        return HashPassword(enteredPassword) == hashedPassword;
    }
}