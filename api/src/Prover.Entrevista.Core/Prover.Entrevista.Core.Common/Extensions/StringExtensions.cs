using System.Security.Cryptography;
using System.Text;

namespace Prover.Entrevista.Core.Common.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value) {  return value.ToLower(); }
    public static string ToSHA256(this string value)
    {
        using var sha256 = SHA256.Create();
        var stringBytes = Encoding.UTF8.GetBytes(value);
        var hashBytes = sha256.ComputeHash(stringBytes);

        return Convert.ToBase64String(hashBytes);

        //StringBuilder sb = new StringBuilder();

        //for (int i = 0; i < hashBytes.Length; i++)
        //    sb.Append(hashBytes[i]);

        //return sb.ToString();
    }
}