using System.Windows.Media;

namespace AdvancedClipboard.Wpf.Extensions
{
  public static class ColorExtensions
  {
    #region Methods

    public static double CalculateLuminance(this Color color)
    {
      return (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) / byte.MaxValue;
    }

    #endregion Methods
  }
}