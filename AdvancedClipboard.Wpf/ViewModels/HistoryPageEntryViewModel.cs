using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.Windows;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Constructors

    public HistoryPageEntryViewModel()
    {
      this.LoadIntoClipboardCommand = new DelegateCommand(this.LoadIntoClipboardCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public string PlainTextContent { get; set; }
    public DelegateCommand LoadIntoClipboardCommand { get; }

    #endregion Properties

    #region Methods

    private void LoadIntoClipboardCommandExecute()
    {
      Clipboard.SetText(PlainTextContent);
    }

    #endregion Methods
  }
}