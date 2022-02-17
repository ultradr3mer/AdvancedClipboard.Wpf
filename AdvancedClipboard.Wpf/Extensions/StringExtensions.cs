using System.Linq;
using System.Text;

namespace AdvancedClipboard.Wpf.Extensions
{
  internal static class StringExtensions
  {
    #region Methods

    public static string TruncateWithEllepsis(this string value, int length)
    {
      if (value.Length > length)
      {
        return value.Substring(0, length - 3) + "...";
      }

      return value;
    }

    public static string TruncateWithEllepsis(this string value, int length, int lines)
    {
      if(value == null)
      {
        return value;
      }

      if (value.Length > length)
      {
        value = value.Substring(0, length - 3) + "...";
      }

      var lineStrings = value.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
      if (lineStrings.Length > lines)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var singleLine in lineStrings.Take(lines - 1))
        {
          stringBuilder.AppendLine(singleLine);
        }
        stringBuilder.AppendLine("...");

        return stringBuilder.ToString();
      }

      return value;
    }

    #endregion Methods
  }
}