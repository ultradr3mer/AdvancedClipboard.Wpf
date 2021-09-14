using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  public class ConfigurationPageViewModel : BaseViewModel, INavigationAware
  {
    #region Fields

    private readonly IUnityContainer container;
    private readonly IRegionManager regionManager;
    private readonly Client client;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public ConfigurationPageViewModel(ClipboardService service, IUnityContainer container, IRegionManager regionManager, Client client)
    {
      this.service = service;
      this.container = container;
      this.regionManager = regionManager;
      this.client = client;
      this.SaveCommand = new DelegateCommand(this.SaveCommandExecute);
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public BindingList<LaneConfigurationViewModel> Lanes { get; set; }

    public BindingList<LaneConfigurationViewModel> DeletedLanes { get; set; }

    public ICommand ReturnCommand { get; set; }

    public ICommand SaveCommand { get; set; }

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
      this.Load();
    }

    private async void Load()
    {
      var lanes = (await this.client.LaneGetAsync()).ToList();
      this.Lanes = new BindingList<LaneConfigurationViewModel>(lanes.Select(o => this.container.Resolve<LaneConfigurationViewModel>().GetWithDataModel(o)).ToList());

      this.Lanes.ListChanged += this.Lanes_ListChanged;
      this.EnsureEmptyLane();

      this.DeletedLanes = new BindingList<LaneConfigurationViewModel>();
    }

    private void Lanes_ListChanged(object sender, ListChangedEventArgs e)
    {
      EnsureEmptyLane();
    }

    private void EnsureEmptyLane()
    {
      if (!this.Lanes.Any(o => string.IsNullOrEmpty(o.Name)))
      {
        this.Lanes.Add(this.container.Resolve<LaneConfigurationViewModel>());
      }
    }

    private void ReturnCommandExecute()
    {
      regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    private async void SaveCommandExecute()
    {
      foreach (var item in this.Lanes)
      {
        if(string.IsNullOrEmpty(item.Name))
        {
          continue;
        }

        if(item.Id == Guid.Empty)
        {
          await this.client.LanePostlaneAsync(new LanePostData() { Color = item.Color.ToHexString(), Name = item.Name });
          continue;
        }

        if (item.IsEdited)
        {
          await this.client.LanePutAsync(new LanePutData() { Id = item.Id, Color = item.Color.ToHexString(), Name = item.Name });
          continue;
        }
      }

      foreach (var item in this.DeletedLanes)
      {
        await this.client.LaneDeleteAsync(item.Id);
      }

      regionManager.RequestNavigate(App.RegionName, new Uri(nameof(HistoryPage), UriKind.Relative));
    }

    #endregion Methods
  }
}