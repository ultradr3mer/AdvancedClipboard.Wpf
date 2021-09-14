using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Data;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class EditPageViewModel : BaseViewModel<ClipboardGetData, ClipboardPutData>, INavigationAware
  {
    #region Fields

    internal const string EntryIdParmeter = "Id";
    private readonly Client client;
    private readonly IUnityContainer container;
    private readonly HistoryPageViewModel historyPageViewModel;
    private readonly LaneEntryViewModel NoLane = new LaneEntryViewModel(null) { Name = "None" };
    private readonly IRegionManager regionManager;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public EditPageViewModel(IRegionManager regionManager, HistoryPageViewModel historyPageViewModel, Client client, ClipboardService service, IUnityContainer container)
    {
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
      this.SaveCommand = new DelegateCommand(this.SaveCommandExecute);
      this.OpenShareUrl = new DelegateCommand(this.OpenShareUrlExecute);
      this.CopyShareUrl = new DelegateCommand(this.CopyShareUrlExecute);
      this.regionManager = regionManager;
      this.historyPageViewModel = historyPageViewModel;
      this.client = client;
      this.service = service;
      this.container = container;
    }

    #endregion Constructors

    #region Properties

    public Guid ContentTypeId { get; set; }
    public ICommand CopyShareUrl { get; }
    public Guid Id { get; set; }
    public Uri ImageSource { get; set; }
    public List<LaneEntryViewModel> Lanes { get; set; }
    public ICommand OpenShareUrl { get; }
    public ICommand ReturnCommand { get; }
    public ICommand SaveCommand { get; }
    public LaneEntryViewModel SelectedLane { get; set; }
    public string ShareUrl { get; set; }
    public string TextContent { get; set; }
    public string FileName { get; set; }
    public Visibility FileIconVisibility { get; set; }

    #endregion Properties

    #region Methods

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.Lanes = new[] { this.NoLane }.Concat(this.service.Lanes.Select(o => this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(o))).ToList();

      this.Id = Guid.Parse(navigationContext.Parameters[EntryIdParmeter].ToString());

      this.SetDataModel(this.historyPageViewModel.Entries.Single(o => o.Id == this.Id).WriteToDataModel());
    }

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      if (data.ContentTypeId == ContentTypes.File)
      {
        this.ImageSource = null;
        this.FileName = data.FileName;
        this.TextContent = null;
        this.ShareUrl = SimpleFileTokenData.CreateUrl(data.FileContentUrl).AbsoluteUri;
      }
      else if (data.ContentTypeId == ContentTypes.PlainText)
      {
        this.ImageSource = null;
        this.FileName = null;
        this.TextContent = data.TextContent;
        this.ShareUrl = null;
      }
      else if (data.ContentTypeId == ContentTypes.Image)
      {
        this.ImageSource = SimpleFileTokenData.CreateUrl(data.FileContentUrl);
        this.FileName = null;
        this.TextContent = null;
        this.ShareUrl = this.ImageSource.AbsoluteUri;
      }

      this.SelectedLane = this.Lanes.FirstOrDefault(o => o.Id == data.LaneId) ?? this.Lanes.First();
    }

    private void CopyShareUrlExecute()
    {
      Clipboard.SetText(this.ShareUrl);
    }

    private void OpenShareUrlExecute()
    {
      Process.Start(ShareUrl);
    }

    private void ReturnCommandExecute()
    {
      this.regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    private async void SaveCommandExecute()
    {
      ClipboardPutData data = this.WriteToDataModel();

      if (this.ContentTypeId == ContentTypes.PlainText)
      {
        data.TextContent = this.TextContent;
      }

      data.LaneId = this.SelectedLane != this.NoLane ? this.SelectedLane.Id : (Guid?)null;

      await this.client.ClipboardPutAsync(data);

      await this.service.Refresh();

      this.regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    #endregion Methods
  }
}