using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
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
    private readonly HistoryPageViewModel historyPageViewModel;
    private readonly Client client;
    private readonly ClipboardService service;
    private readonly IUnityContainer container;
    private readonly IRegionManager regionManager;
    private readonly LaneViewModel NoLane = new LaneViewModel(null) { Name = "None" };

    #endregion Fields

    #region Constructors

    public EditPageViewModel(IRegionManager regionManager, HistoryPageViewModel historyPageViewModel, Client client, ClipboardService service, IUnityContainer container)
    {
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
      this.SaveCommand = new DelegateCommand(this.SaveCommandExecute);
      this.regionManager = regionManager;
      this.historyPageViewModel = historyPageViewModel;
      this.client = client;
      this.service = service;
      this.container = container;
    }

    #endregion Constructors

    #region Properties

    public Guid Id { get; set; }
    public List<LaneViewModel> Lanes { get; set; }
    public LaneViewModel SelectedLane { get; set; }
    public ICommand ReturnCommand { get; }
    public ICommand SaveCommand { get; }
    public string Text { get; set; }
    public Guid ContentTypeId { get; set; }

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
      this.Lanes = new[] { this.NoLane }.Concat(this.service.Lanes.Select(o => this.container.Resolve<LaneViewModel>().GetWithDataModel(o))).ToList();

      this.Id = Guid.Parse(navigationContext.Parameters[EntryIdParmeter].ToString());

      this.SetDataModel(this.historyPageViewModel.Entries.Single(o => o.Id == this.Id).WriteToDataModel());
    }

    protected override void OnReadingDataModel(ClipboardGetData data)
    {
      if (data.ContentTypeId == ContentTypes.File)
      {
        this.Text = data.FileName;
      }
      else if (data.ContentTypeId == ContentTypes.PlainText)
      {
        this.Text = data.TextContent;
      }
      else
      {
        throw new NotImplementedException();
      }

      this.SelectedLane = this.Lanes.FirstOrDefault(o => o.Id == data.LaneId) ?? this.Lanes.First();
    }

    private void ReturnCommandExecute()
    {
      this.regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    private async void SaveCommandExecute()
    {
      ClipboardPutData data = this.WriteToDataModel();

      if (this.ContentTypeId == ContentTypes.File)
      {
        data.FileName = this.Text;
      }
      else if (this.ContentTypeId == ContentTypes.PlainText)
      {
        data.TextContent = this.Text;
      }

      data.LaneId = this.SelectedLane != this.NoLane ? this.SelectedLane.Id : (Guid?)null;

      await this.client.ClipboardPutAsync(data);

      await this.service.Refresh();

      this.regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    #endregion Methods
  }
}