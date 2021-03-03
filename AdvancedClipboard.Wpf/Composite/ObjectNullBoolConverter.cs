using System;
using System.Windows.Data;

namespace AdvancedClipboard.Wpf.Composite
{
  public class ObjectNullBoolConverter : IValueConverter
  {
    #region Methods

    /// <summary>Returns false if object is null.</summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return value != null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #endregion Methods
  }
}