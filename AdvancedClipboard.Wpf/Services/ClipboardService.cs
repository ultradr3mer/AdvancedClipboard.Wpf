using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using Prism.Events;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AdvancedClipboard.Wpf.Services
{
  public class ClipboardService
  {
    #region Fields

    private readonly Client client;
    private readonly string tempPath;

    #endregion Fields

    #region Constructors

    public ClipboardService(Client client)
    {
      this.client = client;

      this.tempPath = Path.Combine(Path.GetTempPath(), "AdvancedClipboard");
    }

    #endregion Constructors

    #region Properties

    public BindingList<ClipboardGetData> ClipboardItems { get; } = new BindingList<ClipboardGetData>();
    public bool IsWatchingClipboard { get; set; }

    #endregion Properties

    #region Methods

    public void AddClipboardContent()
    {
      string textContent;
      BitmapSource imageContent;
      IDataObject data = Clipboard.GetDataObject();
      if (data.GetDataPresent(DataFormats.FileDrop))
      {
        var files = (string[])data.GetData(DataFormats.FileDrop);
        this.PostFilesAsync(files);
      }
      if (!string.IsNullOrEmpty(textContent = Clipboard.GetText()))
      {
        this.PostPlaintextAsync(textContent);
      }
      else if ((imageContent = Clipboard.GetImage()) != null)
      {
        this.PostImageAsync(imageContent);
      }
    }

    private async void PostFilesAsync(string[] files)
    {
      foreach (var file in files)
      {
        var fileName = Path.GetFileName(file);
        using var stream = File.OpenRead(file);
        var result = await this.client.ClipboardPostnamedfileAsync(fileName, new FileParameter(stream));
        this.ClipboardItems.Insert(0, result);
      }
    }

    public async Task Delete(Guid id)
    {
      await this.client.ClipboardDeleteAsync(id);

      var itemToRemove = ClipboardItems.FirstOrDefault(o => o.Id == id);
      this.ClipboardItems.Remove(itemToRemove);
    }

    public async Task Refresh()
    {
      this.ClipboardItems.Clear();

      var data = await client.ClipboardGetAsync();
      foreach (var item in data)
      {
        this.ClipboardItems.Insert(0, item);
      }
    }

    internal async void SendToClipboard(ClipboardGetData clipboardGetData)
    {
      if (clipboardGetData.ContentTypeId == ContentTypes.Image)
      {
        var url = SimpleFileTokenData.CreateUrl(clipboardGetData.FileContentUrl);
        WebRequest request = WebRequest.Create(url);
        using WebResponse response = request.GetResponse();
        using Stream responseStream = response.GetResponseStream();
        var bi = BitmapFrame.Create(responseStream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        bi.DownloadCompleted += (o, a) => Clipboard.SetImage(bi);
      }
      if (clipboardGetData.ContentTypeId == ContentTypes.File)
      {
        if (!Directory.Exists(this.tempPath))
        {
          Directory.CreateDirectory(this.tempPath);
        }

        var url = SimpleFileTokenData.CreateUrl(clipboardGetData.FileContentUrl);
        using (var client = new WebClient())
        {
          string path = Path.Combine(this.tempPath, clipboardGetData.FileName);
          await client.DownloadFileTaskAsync(url, path);
          StringCollection paths = new StringCollection();
          paths.Add(path);
          Clipboard.SetFileDropList(paths);
        }
      }
      else if (clipboardGetData.ContentTypeId == ContentTypes.PlainText)
      {
        Clipboard.SetText(clipboardGetData.TextContent);
      }
    }

    private async void PostImageAsync(BitmapSource imageContent)
    {
      using var memoryStream = new MemoryStream();

      BitmapEncoder encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(imageContent));
      encoder.Save(memoryStream);
      memoryStream.Seek(0, SeekOrigin.Begin);

      memoryStream.Seek(0, SeekOrigin.Begin);

      var result = await this.client.ClipboardPostfileAsync(".png", new FileParameter(memoryStream));
      this.ClipboardItems.Insert(0, result);
    }

    private async void PostPlaintextAsync(string textContent)
    {
      var result = await this.client.ClipboardPostplaintextAsync(new ClipboardPostPlainTextData() { Content = textContent });
      this.ClipboardItems.Insert(0, result);
    }

    #endregion Methods
  }
}