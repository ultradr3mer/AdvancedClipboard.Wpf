using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System.ComponentModel;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  internal class EntriesDesignTimeViewModel : BaseViewModel
  {
    #region Constructors

    public EntriesDesignTimeViewModel()
    {
      this.Entries = new BindingList<HistoryPageEntryViewModel>()
      {
        this.CreateTextEntry("Erster Text"),
        this.CreateImageEntry("F451D57C64FC6140/clip_20210413_135842.png"),
        this.CreateFileEntry("3CDAB6DEE7BE995B/clip_20210414_133243.zip", "datei.zip"),
        this.CreateTextEntry("Zweiter Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text."),
        this.CreateTextEntry("Anderer Text"),
        this.CreateTextEntry("Noch ein Text"),
        this.CreateImageEntry("26C8C736070D7EFF/clip_20210316_213458.jpg"),
        this.CreateTextEntry("Dieser Text"),
        this.CreateTextEntry("Der andere Text")
      };
    }

    #endregion Constructors

    #region Properties

    public BindingList<HistoryPageEntryViewModel> Entries { get; set; }

    #endregion Properties

    #region Methods

    private HistoryPageEntryViewModel CreateFileEntry(string url, string fileName)
    {
      return new HistoryPageEntryViewModel(null,null).GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.File, FileName = fileName });
    }

    private HistoryPageEntryViewModel CreateImageEntry(string url)
    {
      return new HistoryPageEntryViewModel(null, null).GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.Image });
    }

    private LaneViewModel CreateLaneEntry(string color, string name)
    {
      return new LaneViewModel(null).GetWithDataModel(new LaneGetData { Color = color, Name = name });
    }

    private HistoryPageEntryViewModel CreateTextEntry(string text)
    {
      return new HistoryPageEntryViewModel(null, null) { TextContent = text };
    }

    #endregion Methods
  }
}