using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Fields

    private readonly ClipboardService clipboardService;

    #endregion Fields

    #region Constructors

    public HistoryPageEntryViewModel(ClipboardService clipboardService)
    {
      this.LoadIntoClipboardCommand = new DelegateCommand(this.LoadIntoClipboardCommandExecute);
      this.clipboardService = clipboardService;
    }

    #endregion Constructors

    #region Properties

    public Uri ImageUrl { get; private set; }

    public DelegateCommand LoadIntoClipboardCommand { get; }

    public string TextContent { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      this.ImageUrl = SimpleFileTokenData.CreateUrl(data.ImageContentUrl);
    }

    private void LoadIntoClipboardCommandExecute()
    {
      if (ImageUrl != null)
      {
        WebRequest request = WebRequest.Create(ImageUrl);
        WebResponse response = request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        var bi = BitmapFrame.Create(responseStream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        bi.DownloadCompleted += (o, a) => Clipboard.SetImage(bi);
      }
      else if (!string.IsNullOrEmpty(TextContent))
      {
        Clipboard.SetText(TextContent);
      }
    }

    #endregion Methods
  }
}