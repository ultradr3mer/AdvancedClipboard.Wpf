using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Interfaces;
using AdvancedClipboard.Wpf.Services;
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
  internal class LanePageViewModel : HistoryPageViewModel, INavigationAware, IEntryHostViewModel
  {
    #region Fields

    public const string LaneIdParameter = "Id";

    #endregion Fields

    #region Constructors

    public LanePageViewModel(IUnityContainer container, ClipboardService service, Client client, IRegionManager regionManager)
      : base(container, service, client, regionManager)
    {
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
    }

    #endregion Constructors

    #region Properties

    public SolidColorBrush BackgroundBrush { get; private set; }

    public SolidColorBrush ForegroundBrush { get; private set; }

    public string Name { get; set; }

    public ICommand ReturnCommand { get; }

    #endregion Properties

    #region Methods

    public override void OnNavigatedFrom(NavigationContext navigationContext)
    {
      this.Entries.Clear();
      this.Lanes.Clear();
      this.BackgroundBrush = null;
      this.ForegroundBrush = null;
      this.Name = string.Empty;

      base.OnNavigatedFrom(navigationContext);
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.LaneId = Guid.Parse(navigationContext.Parameters[LanePageViewModel.LaneIdParameter].ToString());

      base.OnNavigatedTo(navigationContext);
    }

    protected override async void Load()
    {
      var data = await this.client.ClipboardGetlanewithcontextAsync(this.LaneId);

      this.SetLane(data.Lanes.Single(o => o.Id == this.LaneId));
      this.Lanes = new BindingList<LaneEntryViewModel>(data.Lanes.Select(o => this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(o)).ToList());
      this.Entries = new BindingList<HistoryPageEntryViewModel>(data.Entries.Reverse().Select(o => this.container.Resolve<HistoryPageEntryViewModel>().SetHost(this).GetWithDataModel(o)).ToList());
    }

    private void ReturnCommandExecute()
    {
      this.regionManager.Regions[App.RegionName].NavigationService.Journal.GoBack();
    }

    protected void SetLane(LaneGetData lane)
    {
      var backColor = (Color)ColorConverter.ConvertFromString(lane.Color);
      this.BackgroundBrush = new SolidColorBrush(backColor);

      var foregroundColor = backColor.CalculateLuminance() > 0.6 ? Colors.Black : Colors.White;
      this.ForegroundBrush = new SolidColorBrush(foregroundColor);

      this.Name = lane.Name;
    }

    #endregion Methods
  }
}