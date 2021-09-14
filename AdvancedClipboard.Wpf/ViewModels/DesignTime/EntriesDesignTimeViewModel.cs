using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  internal class EntriesDesignTimeViewModel : BaseViewModel
  {
    #region Constructors

    public EntriesDesignTimeViewModel()
    {
      var colors = new[]
      {
        this.CreateBrush("#FFDC143C"),
        this.CreateBrush("#FF9400D3"),
        this.CreateBrush("#FF6495ED"),
        this.CreateBrush("#FFFFFFFF")
      };

      this.Entries = new BindingList<HistoryPageEntryViewModel>()
      {
        this.CreateTextEntry("Erster Text", colors[0]),
        this.CreateImageEntry("F451D57C64FC6140/clip_20210413_135842.png"),
        this.CreateFileEntry("3CDAB6DEE7BE995B/clip_20210414_133243.zip", "datei.zip", colors[1]),
        this.CreateTextEntry("Zweiter Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text.", colors[2]),
        this.CreateTextEntry("Anderer Text", colors[3]),
        this.CreateTextEntry("Noch ein Text", colors[0]),
        this.CreateImageEntry("26C8C736070D7EFF/clip_20210316_213458.jpg", colors[2]),
        this.CreateTextEntry("Dieser Text"),
        this.CreateTextEntry("Der andere Text")
      };
    }

    private SolidColorBrush CreateBrush(string hex)
    {
      var backColor = (Color)ColorConverter.ConvertFromString(hex);
      return new SolidColorBrush(backColor);
    }

    #endregion Constructors

    #region Properties

    public BindingList<HistoryPageEntryViewModel> Entries { get; set; }

    #endregion Properties

    #region Methods

    private HistoryPageEntryViewModel CreateFileEntry(string url, string fileName, SolidColorBrush laneColorBrush = default(SolidColorBrush))
    {
      return new HistoryPageEntryViewModel(null, null, null) { LaneColorBrush = laneColorBrush }.GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.File, FileName = fileName });
    }

    private HistoryPageEntryViewModel CreateImageEntry(string url, SolidColorBrush laneColorBrush = default(SolidColorBrush))
    {
      return new HistoryPageEntryViewModel(null, null, null) { LaneColorBrush = laneColorBrush }.GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.Image });
    }

    private HistoryPageEntryViewModel CreateTextEntry(string text, SolidColorBrush laneColorBrush = default(SolidColorBrush))
    {
      return new HistoryPageEntryViewModel(null, null, null) { LaneColorBrush = laneColorBrush, TextContent = text };
    }

    #endregion Methods
  }
}