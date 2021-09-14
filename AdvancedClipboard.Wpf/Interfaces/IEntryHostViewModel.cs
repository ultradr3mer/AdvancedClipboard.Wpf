using AdvancedClipboard.Wpf.ViewModels;
using System.ComponentModel;

namespace AdvancedClipboard.Wpf.Interfaces
{
  public interface IEntryHostViewModel
  {
    #region Properties

    BindingList<HistoryPageEntryViewModel> Entries { get; }
    BindingList<LaneEntryViewModel> Lanes { get; }

    #endregion Properties
  }
}