using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Constructors

    public HistoryPageEntryViewModel()
    {
      this.LoadIntoClipboardCommand = new DelegateCommand(this.LoadIntoClipboardCommandExecute);
    }

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      this.ImageUrl = SimpleFileTokenData.CreateUrl(data.ImageContentUrl);
    }

    #endregion Constructors

    #region Properties

    public string TextContent { get; set; }
    public DelegateCommand LoadIntoClipboardCommand { get; }
    public Uri ImageUrl { get; private set; }

    #endregion Properties

    #region Methods

    private void LoadIntoClipboardCommandExecute()
    {
      if(ImageUrl != null)
      {
        BitmapImage image = new BitmapImage();
        image.BeginInit();
        image.UriSource = ImageUrl;
        image.EndInit();
        Clipboard.SetImage(image);
      }
      else if(!string.IsNullOrEmpty(TextContent))
      {
        Clipboard.SetText(TextContent);
      }
    }

    #endregion Methods
  }
}