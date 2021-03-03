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
        new HistoryPageEntryViewModel(){Title = "Erster Text"},
        new HistoryPageEntryViewModel(){Title = "Zweiter Text"},
        new HistoryPageEntryViewModel(){Title = "Anderer Text"},
        new HistoryPageEntryViewModel(){Title = "Noch ein Text"},
        new HistoryPageEntryViewModel(){Title = "Dieser Text"},
        new HistoryPageEntryViewModel(){Title = "Der andere Text"},
      };
    }

    #endregion Constructors

    #region Methods

    protected override void LoadAsync()
    {

    }

    #endregion Methods
  }
}