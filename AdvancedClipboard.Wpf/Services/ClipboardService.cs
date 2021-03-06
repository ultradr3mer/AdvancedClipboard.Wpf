using AdvancedClipboard.Wpf.Events;
using ManagedWinapi;
using Prism.Events;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace AdvancedClipboard.Wpf.Services
{
  internal class ClipboardService
  {
    #region Fields

    private readonly Client client;
    private readonly ClipboardChangedEvent changedEvent;
    private string lastText;

    #endregion Fields

    #region Constructors

    public ClipboardService(ClipboardNotifier notifier, Client client, IEventAggregator eventAggregator)
    {
      notifier.ClipboardChanged += this.NotifierClipboard;
      this.client = client;
      this.changedEvent = eventAggregator.GetEvent<ClipboardChangedEvent>();
      this.changedEvent.Subscribe(this.NotifierClipboardSta, ThreadOption.UIThread);
      this.LoadAsync();
    }

    private void NotifierClipboardSta(ClipboardChangedData obj)
    {
      string textContent;
      if (!string.IsNullOrEmpty(textContent = Clipboard.GetText()) && textContent != this.lastText)
      {
        this.lastText = textContent;
        this.PostPlaintextAsync(textContent);
      }
    }

    private void NotifierClipboard(object sender, EventArgs e)
    {
      // Publish event to perform further processing on the UI thread.
      this.changedEvent.Publish(new ClipboardChangedData());
    }

    private async void PostPlaintextAsync(string textContent)
    {
      var result = await this.client.ClipboardPostplaintextAsync(new ClipboardPostPlainTextData() { Content = textContent });
      this.ClipboardItems.Insert(0, new ClipboardGetData() { Id = result.Id, PlainTextContent = textContent });
    }

    #endregion Constructors

    #region Properties

    public BindingList<ClipboardGetData> ClipboardItems { get; } = new BindingList<ClipboardGetData>();

    #endregion Properties

    #region Methods

    protected virtual async void LoadAsync()
    {
      var data = await client.ClipboardAsync();
      foreach (var item in data)
      {
        this.ClipboardItems.Insert(0, item);
        this.lastText = item.PlainTextContent;
      }
    }

    #endregion Methods
  }
}