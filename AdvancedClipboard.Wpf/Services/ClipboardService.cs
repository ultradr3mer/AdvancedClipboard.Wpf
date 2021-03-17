using AdvancedClipboard.Wpf.Events;
using ManagedWinapi;
using Prism.Events;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AdvancedClipboard.Wpf.Services
{
  public class ClipboardService
  {
    #region Fields

    private readonly ClipboardChangedEvent changedEvent;
    private readonly Client client;
    private string lastText;
    private BitmapSource lastImage;

    #endregion Fields

    #region Constructors

    public ClipboardService(ClipboardNotifier notifier, Client client, IEventAggregator eventAggregator)
    {
      notifier.ClipboardChanged += this.NotifierClipboard;
      this.client = client;
      this.changedEvent = eventAggregator.GetEvent<ClipboardChangedEvent>();
      this.changedEvent.Subscribe(this.NotifierClipboardSta, ThreadOption.UIThread);
    }

    #endregion Constructors

    #region Properties

    public BindingList<ClipboardGetData> ClipboardItems { get; } = new BindingList<ClipboardGetData>();
    public bool IsWatchingClipboard { get; set; }

    #endregion Properties

    #region Methods

    public async Task Refresh()
    {
      this.ClipboardItems.Clear();

      var data = await client.ClipboardAsync();
      foreach (var item in data)
      {
        this.ClipboardItems.Insert(0, item);
        this.lastText = item.TextContent;
      }
    }

    private void NotifierClipboard(object sender, EventArgs e)
    {
      if (!this.IsWatchingClipboard)
      {
        return;
      }

      // Publish event to perform further processing on the UI thread.
      this.changedEvent.Publish(new ClipboardChangedData());
    }

    private void NotifierClipboardSta(ClipboardChangedData obj)
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
        this.lastImage = imageContent;
        this.PostImageAsync(imageContent);
      }
    }

    private async void PostImageAsync(BitmapSource imageContent)
    {
      using var memoryStream = new MemoryStream();

      BitmapEncoder encoder = new PngBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(imageContent));
      encoder.Save(memoryStream);
      memoryStream.Seek(0, SeekOrigin.Begin);

      var result = await this.client.ClipboardPostimageAsync(".png", new FileParameter(memoryStream));
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