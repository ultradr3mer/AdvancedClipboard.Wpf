using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.IO;
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

    public Uri ImageUrl { get; set; }

    public ICommand LoadIntoClipboardCommand { get; }

    public string TextContent { get; set; }

    public Guid Id { get; set; }

    public Uri FileContentUrl { get; set; }

    public string FileName { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      if (data.ContentTypeId == ContentTypes.Image)
      {
        this.ImageUrl = SimpleFileTokenData.CreateUrl(data.FileContentUrl);
      }
      else if(data.ContentTypeId == ContentTypes.File)
      {
        this.FileContentUrl = SimpleFileTokenData.CreateUrl(data.FileContentUrl);
      }
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