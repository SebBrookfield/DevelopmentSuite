namespace Development.Suite.Common.ExtensionMethods;

public static class StringExtensions
{
    public static bool IsNullOrWhitespace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}