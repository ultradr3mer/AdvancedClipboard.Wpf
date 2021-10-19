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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly LaneEntryViewModel NoLane = new LaneEntryViewModel(null) { Name = "None" };
    private readonly IRegionManager regionManager;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public EditPageViewModel(IRegionManager regionManager, Client client, ClipboardService service, IUnityContainer container)
    {
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
      this.SaveCommand = new DelegateCommand(this.SaveCommandExecute);
      this.OpenShareUrl = new DelegateCommand(this.OpenShareUrlExecute);
      this.CopyShareUrl = new DelegateCommand(this.CopyShareUrlExecute);
      this.regionManager = regionManager;
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
    public List<LaneEntryViewModel> Lanes { get; set; } = new List<LaneEntryViewModel>();
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
      this.Lanes.Clear();
      this.SetDataModel(null);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.Id = Guid.Parse(navigationContext.Parameters[EntryIdParmeter].ToString());

      this.Load();
    }

    private async void Load()
    {
      var data = await this.client.ClipboardGetwithcontextAsync(this.Id);

      this.Lanes = new[] { NoLane }.Concat(data.Lanes.Select(o => this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(o))).ToList();
      this.SetDataModel(data.Entries.Single());
    }

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      if(data == null)
      {
        this.ImageSource = null;
        this.FileName = null;
        this.TextContent = null;
        this.ShareUrl = null;
        this.SelectedLane = null;

        return;
      }

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
      this.regionManager.Regions[App.RegionName].NavigationService.Journal.GoBack();
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

      this.ReturnCommandExecute();
    }

    #endregion Methods
  }
}