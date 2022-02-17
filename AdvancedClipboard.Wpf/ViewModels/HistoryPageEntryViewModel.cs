using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Interfaces;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Linq;
using System.Net.Cache;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
      this.ConfirmNo = new DelegateCommand(this.ConfirmNoExecute);
      this.ConfirmYes = new DelegateCommand(this.ConfirmYesExecute);

      this.clipboardService = clipboardService;
      this.regionManager = regionManager;
      this.client = client;

      this.ConfirmVisibility = Visibility.Collapsed;
    }

    #endregion Constructors

    #region Properties

    public ICommand ConfirmNo { get; }

    public Visibility ConfirmVisibility { get; set; }

    public ICommand ConfirmYes { get; }

    public ICommand DeleteCommand { get; }

    public ICommand EditCommand { get; }

    public Uri FileContentUrl { get; set; }

    public string FileName { get; set; }

    public Guid Id { get; set; }

    public ImageSource ImageSource { get; set; }

    public SolidColorBrush LaneColorBrush { get; internal set; }

    public ICommand LoadIntoClipboardCommand { get; }

    public string TextContent { get; set; }

    public string TextContentShort { get; set; }

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
        BitmapImage bitmapImage = new BitmapImage(SimpleFileTokenData.CreateUrl(data.FileContentUrl), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
        this.ImageSource = bitmapImage;
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

      this.TextContentShort = this.TextContent.TruncateWithEllepsis(700, 8);
    }

    private void ConfirmNoExecute()
    {
      this.ConfirmVisibility = Visibility.Collapsed;
    }

    private async void ConfirmYesExecute()
    {
      await this.client.ClipboardDeleteAsync(this.Id);

      this.hostViewModel.Entries.Remove(this);
    }

    private void DeleteCommandExecute()
    {
      this.ConfirmVisibility = Visibility.Visible;
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