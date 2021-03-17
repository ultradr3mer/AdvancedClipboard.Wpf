using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
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
        new HistoryPageEntryViewModel(){TextContent = "Erster Text"},
        new HistoryPageEntryViewModel().GetWithDataModel(new ClipboardGetData{ImageContentUrl = "EFE315E4553BBAFA/clip_20210316_213415.JPG"}),
        new HistoryPageEntryViewModel(){TextContent = "Zweiter Text"},
        new HistoryPageEntryViewModel(){TextContent = "Anderer Text"},
        new HistoryPageEntryViewModel(){TextContent = "Noch ein Text"},
        new HistoryPageEntryViewModel().GetWithDataModel(new ClipboardGetData{ImageContentUrl = "26C8C736070D7EFF/clip_20210316_213458.jpg"}),
        new HistoryPageEntryViewModel(){TextContent = "Dieser Text"},
        new HistoryPageEntryViewModel(){TextContent = "Der andere Text"},
      };
    }

    #endregion Constructors

    #region Methods

    protected override void Load()
    {
    }

    #endregion Methods
  }
}