using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.Windows.Input;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Fields

    private readonly ClipboardService clipboardService;

    #endregion Fields

    #region Constructors

    public HistoryPageEntryViewModel(ClipboardService clipboardService)
    {
      this.LoadIntoClipboardCommand = new DelegateCommand(this.LoadIntoClipboardCommandExecute);
      this.DeleteCommand = new DelegateCommand(this.DeleteCommandExecute);

      this.clipboardService = clipboardService;
    }

    #endregion Constructors

    #region Properties

    public ICommand DeleteCommand { get; }

    public Uri ImageUrl { get; private set; }

    public ICommand LoadIntoClipboardCommand { get; }

    public string TextContent { get; set; }

    public Guid Id { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      this.ImageUrl = SimpleFileTokenData.CreateUrl(data.ImageContentUrl);
    }

    private async void DeleteCommandExecute()
    {
      await this.clipboardService.Delete(this.Id);
    }

    private void LoadIntoClipboardCommandExecute()
    {
      this.clipboardService.SendToClipboard(this.WriteToDataModel());
    }

    #endregion Methods
  }
}