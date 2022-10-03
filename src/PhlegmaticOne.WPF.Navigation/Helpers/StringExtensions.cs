using System.Text.RegularExpressions;

namespace PhlegmaticOne.WPF.Navigation.Helpers;

internal static class StringExtensions
{
    internal static string[] SplitByUppercase(this string str) =>
        Regex.Split(str, Constants.SplitByUppercaseRegex);
}
