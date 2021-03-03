using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Services;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Properties

    public string PlainTextContent { get; set; }

    #endregion Properties
  }
}