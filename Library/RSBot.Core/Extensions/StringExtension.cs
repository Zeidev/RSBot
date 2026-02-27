using System;
using System.Text;

namespace RSBot.Core.Extensions;

public static class StringExtension
{
    #region Methods

    public static string JoymaxFormat<T1>(this string unformatted, T1 valA)
    {
        var stringBulder = new StringBuilder(unformatted);
        stringBulder = stringBulder.Replace("%d", "%s");

        stringBulder = stringBulder.Replace("%s", valA.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);

        return stringBulder.ToString();
    }

    public static string JoymaxFormat<T1, T2>(this string unformatted, T1 valA, T2 valB)
    {
        var stringBulder = new StringBuilder(unformatted);
        stringBulder = stringBulder.Replace("%d", "%s");

        stringBulder = stringBulder.Replace("%s", valA.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);
        stringBulder = stringBulder.Replace("%s", valB.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);

        return stringBulder.ToString();
    }

    public static string JoymaxFormat<T1, T2, T3>(this string unformatted, T1 valA, T2 valB, T3 valC)
    {
        var stringBulder = new StringBuilder(unformatted);
        stringBulder = stringBulder.Replace("%d", "%s");

        stringBulder = stringBulder.Replace("%s", valA.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);
        stringBulder = stringBulder.Replace("%s", valB.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);
        stringBulder = stringBulder.Replace("%s", valC.ToString(), 0, stringBulder.ToString().IndexOf("%s") + 2);

        return stringBulder.ToString();
    }

    /// <summary>
    /// Checks if string is null, empty, or whitespace.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Returns the string or a default value if null or empty.
    /// </summary>
    public static string OrDefault(this string value, string defaultValue = "")
    {
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }

    /// <summary>
    /// Safely formats string with arguments (null-safe).
    /// </summary>
    public static string SafeFormat(this string format, params object[] args)
    {
        if (string.IsNullOrEmpty(format))
            return string.Empty;

        try
        {
            return string.Format(format, args);
        }
        catch
        {
            return format;
        }
    }

    /// <summary>
    /// Truncates string to specified length.
    /// </summary>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    #endregion Methods
}
