using AdvancedClipboard.Wpf.Data;
using Prism.Events;
using System;
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
    private string lastImageMd5;
    private string lastText;

    #endregion Fields

    #region Constructors

    public ClipboardService(Client client)
    {
      this.client = client;
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
      if (!string.IsNullOrEmpty(textContent = Clipboard.GetText()))
      {
        if (textContent != this.lastText)
        {
          this.lastText = textContent;
          this.PostPlaintextAsync(textContent);
        }
      }
      else if ((imageContent = Clipboard.GetImage()) != null)
      {
        this.PostImageAsync(imageContent);
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
        this.lastText = item.TextContent;
      }
    }

    internal void SendToClipboard(ClipboardGetData clipboardGetData)
    {
      if (clipboardGetData.ImageContentUrl != null)
      {
        var url = SimpleFileTokenData.CreateUrl(clipboardGetData.ImageContentUrl);
        WebRequest request = WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        var bi = BitmapFrame.Create(responseStream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
        bi.DownloadCompleted += (o, a) => Clipboard.SetImage(bi);
      }
      else if (!string.IsNullOrEmpty(clipboardGetData.TextContent))
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

      var md5 = MD5.Create();
      var currentImageMd5 = Convert.ToBase64String(md5.ComputeHash(memoryStream));
      memoryStream.Seek(0, SeekOrigin.Begin);

      if (this.lastImageMd5 != currentImageMd5)
      {
        this.lastImageMd5 = currentImageMd5;
        var result = await this.client.ClipboardPostimageAsync(".png", new FileParameter(memoryStream));
        this.ClipboardItems.Insert(0, result);
      }
    }

    private async void PostPlaintextAsync(string textContent)
    {
      var result = await this.client.ClipboardPostplaintextAsync(new ClipboardPostPlainTextData() { Content = textContent });
      this.ClipboardItems.Insert(0, result);
    }

    #endregion Methods
  }
}