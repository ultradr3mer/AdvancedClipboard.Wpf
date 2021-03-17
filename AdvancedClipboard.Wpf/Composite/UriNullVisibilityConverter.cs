using System;
using System.Windows;
using System.Windows.Data;

namespace AdvancedClipboard.Wpf.Composite
{
  public class UriNullVisibilityConverter : IValueConverter
  {
    #region Methods

    /// <summary>Returns false if string is null or empty.</summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var s = value as Uri;
      return s == null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion Methods
  }
}