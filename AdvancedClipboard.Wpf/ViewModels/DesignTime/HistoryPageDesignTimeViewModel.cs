using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System;
using System.ComponentModel;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  internal class HistoryPageDesignTimeViewModel : HistoryPageViewModel
  {
    #region Constructors

    public HistoryPageDesignTimeViewModel() : base(null, null)
    {
      this.Entrys = new BindingList<HistoryPageEntryViewModel>()
      {
        this.CreateTextEntry("Erster Text"),
        this.CreateImageEntry("EFE315E4553BBAFA/clip_20210316_213415.JPG"),
        this.CreateTextEntry("Zweiter Text"),
        this.CreateTextEntry("Anderer Text"),
        this.CreateTextEntry("Noch ein Text"),
        this.CreateImageEntry("26C8C736070D7EFF/clip_20210316_213458.jpg"),
        this.CreateTextEntry("Dieser Text"),
        this.CreateTextEntry("Der andere Text")
      };
    }

    private HistoryPageEntryViewModel CreateImageEntry(string url)
    {
      return new HistoryPageEntryViewModel(null).GetWithDataModel(new ClipboardGetData { ImageContentUrl = url });
    }

    private HistoryPageEntryViewModel CreateTextEntry(string text)
    {
      return new HistoryPageEntryViewModel(null) { TextContent = text };
    }

    #endregion Constructors

    #region Methods

    protected override void Load()
    {
    }

    #endregion Methods
  }
}