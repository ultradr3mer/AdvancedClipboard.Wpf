using System;

namespace AdvancedClipboard.Wpf.Data
{
  public static class SimpleFileTokenData
  {
    #region Fields

    private const string BaseUrl = "https://advancedclipboard.azurewebsites.net/file/";

    #endregion Fields

    #region Methods

    public static Uri CreateUrl(string localUrl)
    {
      return string.IsNullOrEmpty(localUrl) ? null : new Uri(BaseUrl + localUrl);
    }

    #endregion Methods
  }
}