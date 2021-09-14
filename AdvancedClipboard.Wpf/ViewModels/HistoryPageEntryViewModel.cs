using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Interfaces;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.ViewModels
{
  public class HistoryPageEntryViewModel : BaseViewModel<ClipboardGetData>
  {
    #region Fields

    public IEntryHostViewModel hostViewModel;
    private readonly Client client;
    private readonly ClipboardService clipboardService;
    private readonly IRegionManager regionManager;

    #endregion Fields

    #region Constructors

    public HistoryPageEntryViewModel(ClipboardService clipboardService, IRegionManager regionManager, Client client)
    {
      this.LoadIntoClipboardCommand = new DelegateCommand(this.LoadIntoClipboardCommandExecute);
      this.DeleteCommand = new DelegateCommand(this.DeleteCommandExecute);
      this.EditCommand = new DelegateCommand(this.EditCommandExecute);

      this.clipboardService = clipboardService;
      this.regionManager = regionManager;
      this.client = client;
    }

    #endregion Constructors

    #region Properties

    public ICommand DeleteCommand { get; }

    public ICommand EditCommand { get; }

    public Uri FileContentUrl { get; set; }

    public string FileName { get; set; }

    public Guid Id { get; set; }

    public Uri ImageUrl { get; set; }

    public SolidColorBrush LaneColorBrush { get; internal set; }

    public ICommand LoadIntoClipboardCommand { get; }

    public string TextContent { get; set; }

    #endregion Properties

    #region Methods

    internal HistoryPageEntryViewModel SetHost(IEntryHostViewModel entryHostViewModel)
    {
      this.hostViewModel = entryHostViewModel;
      return this;
    }

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      if (data.ContentTypeId == ContentTypes.Image)
      {
        this.ImageUrl = SimpleFileTokenData.CreateUrl(data.FileContentUrl);
        this.FileName = string.Empty;
      }
      else if (data.ContentTypeId == ContentTypes.File)
      {
        this.FileContentUrl = SimpleFileTokenData.CreateUrl(data.FileContentUrl);
      }

      LaneEntryViewModel laneGetData;
      if (data.LaneId != null && (laneGetData = this.hostViewModel.Lanes.FirstOrDefault(o => o.Id == data.LaneId)) != null)
      {
        var color = (Color)ColorConverter.ConvertFromString(laneGetData.Color);
        this.LaneColorBrush = new SolidColorBrush(color);
      }
      else
      {
        var color = (Color)ColorConverter.ConvertFromString("#5effffff");
        this.LaneColorBrush = new SolidColorBrush(color);
      }
    }

    private async void DeleteCommandExecute()
    {
      if (MessageBox.Show("Are you sure you want to delete this entry?",
                         "Advanced Clipboard",
                         MessageBoxButton.OKCancel,
                         MessageBoxImage.Warning) != MessageBoxResult.OK)
      {
        return;
      }

      await this.client.ClipboardDeleteAsync(this.Id);
    }

    private void EditCommandExecute()
    {
      var navigationParameters = new NavigationParameters();
      navigationParameters.Add(EditPageViewModel.EntryIdParmeter, this.Id);
      regionManager.RequestNavigate(App.RegionName, new Uri(nameof(EditPage) + navigationParameters.ToString(), UriKind.Relative));
    }

    private void LoadIntoClipboardCommandExecute()
    {
      this.clipboardService.SendToClipboard(this.WriteToDataModel());
    }

    #endregion Methods
  }
}