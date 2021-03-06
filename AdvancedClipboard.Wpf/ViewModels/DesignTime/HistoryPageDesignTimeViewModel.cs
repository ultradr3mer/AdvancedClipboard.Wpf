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
        new HistoryPageEntryViewModel(){PlainTextContent = "Erster Text"},
        new HistoryPageEntryViewModel(){PlainTextContent = "Zweiter Text"},
        new HistoryPageEntryViewModel(){PlainTextContent = "Anderer Text"},
        new HistoryPageEntryViewModel(){PlainTextContent = "Noch ein Text"},
        new HistoryPageEntryViewModel(){PlainTextContent = "Dieser Text"},
        new HistoryPageEntryViewModel(){PlainTextContent = "Der andere Text"},
      };
    }

    #endregion Constructors
  }
}