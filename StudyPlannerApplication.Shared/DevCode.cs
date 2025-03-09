using System.Text;
using System.Security.Cryptography;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace StudyPlannerApplication.Shared;

public static partial class DevCode
{
    public static string ToSHA256HexHashString(this string password, string mobileNo)
    {
        password = password.Trim();
        mobileNo = mobileNo.Trim();
        string saltedCode = EncodedBySalted(mobileNo); //salted user name
        string hashString;
        using (var sha256 = SHA256Managed.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(password + saltedCode));
            hashString = ToHex(hash, false);
        }

        return hashString;
    }

    private static string EncodedBySalted(string decodeString)
    {
        decodeString = decodeString.ToLower()
            .Replace("a", "@")
            .Replace("i", "!")
            .Replace("l", "1")
            .Replace("e", "3")
            .Replace("o", "0")
            .Replace("s", "$")
            .Replace("n", "&");
        return decodeString;
    }

    public static string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        return result.ToString();
    }

    public static string ToEnumDescription<T>(this T val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }

    public static string GeneratePassword()
    {
        var random = new Random();
        var rNum = new StringBuilder();
        for (int i = 0; i < 6; i++)
        {
            rNum.Append(random.Next(0, 6).ToString());
        }

        return rNum.ToString();
    }

    public static void LogCustomError(this ILogger logger,
        Exception ex,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "",
        [CallerLineNumber] int lineNo=0)
    {
        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Line Number - {lineNo} | Result - {ex}";
        logger.LogError(message);
    }
}
