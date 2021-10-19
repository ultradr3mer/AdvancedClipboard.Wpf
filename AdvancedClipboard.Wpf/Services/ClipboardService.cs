using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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

      if (!Directory.Exists(this.tempPath))
      {
        Directory.CreateDirectory(this.tempPath);
      }
      else
      {
        foreach (var file in Directory.EnumerateFiles(this.tempPath))
        {
          File.Delete(file);
        }
      }
    }

    #endregion Constructors

    #region Methods

    public async Task<IList<ClipboardGetData>> AddClipboardContent(Guid? laneId)
    {
      string textContent;
      BitmapSource imageContent;
      IDataObject data = Clipboard.GetDataObject();
      if (data.GetDataPresent(DataFormats.FileDrop))
      {
        var files = (string[])data.GetData(DataFormats.FileDrop);
        return await this.PostFilesAsync(files, laneId);
      }
      if (!string.IsNullOrEmpty(textContent = Clipboard.GetText()))
      {
        return new List<ClipboardGetData> { await this.PostPlaintextAsync(textContent, laneId) };
      }
      else if ((imageContent = Clipboard.GetImage()) != null)
      {
        return new List<ClipboardGetData> { await this.PostImageAsync(imageContent, laneId) };
      }

      return new List<ClipboardGetData>();
    }

    internal async Task<ClipboardGetData> AddClipboardContent(string textInput, Guid? laneId)
    {
      return await this.PostPlaintextAsync(textInput, laneId);
    }

    internal async void SendToClipboard(ClipboardGetData clipboardGetData)
    {
      if (clipboardGetData.ContentTypeId == ContentTypes.Image)
      {
        var url = SimpleFileTokenData.CreateUrl(clipboardGetData.FileContentUrl);
        var path = Path.Combine(this.tempPath, Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + Path.GetExtension(clipboardGetData.FileContentUrl));

        using (WebClient client = new WebClient())
        {
          await client.DownloadFileTaskAsync(url, path);
        }

        var bs = new BitmapImage(new Uri(path));
        Clipboard.SetImage(bs);
      }
      if (clipboardGetData.ContentTypeId == ContentTypes.File)
      {
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

    private async Task<IList<ClipboardGetData>> PostFilesAsync(string[] files, Guid? laneId)
    {
      var result = new List<ClipboardGetData>();
      foreach (var file in files)
      {
        var fileName = Path.GetFileName(file);
        using var stream = File.OpenRead(file);
        result.Add(await this.client.ClipboardPostnamedfileAsync(fileName, laneId, new FileParameter(stream)));
      }
      return result;
    }

    private async Task<ClipboardGetData> PostImageAsync(BitmapSource imageContent, Guid? laneId)
    {
      using var memoryStream = new MemoryStream();

      BitmapEncoder encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(imageContent));
      encoder.Save(memoryStream);
      memoryStream.Seek(0, SeekOrigin.Begin);

      memoryStream.Seek(0, SeekOrigin.Begin);

      return await this.client.ClipboardPostfileAsync(".png", laneId, new FileParameter(memoryStream));
    }

    private async Task<ClipboardGetData> PostPlaintextAsync(string textContent, Guid? laneId)
    {
      return await this.client.ClipboardPostplaintextAsync(new ClipboardPostPlainTextData() { Content = textContent, LaneGuid = laneId });
    }

    #endregion Methods
  }
}