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
  internal class LanePageViewModel : BaseViewModel<LaneGetData>, INavigationAware
  {
    #region Fields

    public const string LaneIdParameter = "Id";
    private readonly Client client;
    private readonly ClipboardService clipboardService;
    private readonly IUnityContainer container;
    private readonly IRegionManager regionManager;

    #endregion Fields

    #region Constructors

    public LanePageViewModel(Client client, ClipboardService clipboardService, IUnityContainer container, IRegionManager regionManager)
    {
      this.client = client;
      this.clipboardService = clipboardService;
      this.container = container;
      this.regionManager = regionManager;
      this.ReturnCommand = new DelegateCommand(this.ReturnCommandExecute);
    }

    private void ReturnCommandExecute()
    {
      this.regionManager.RequestNavigate(App.RegionName, nameof(HistoryPage));
    }

    #endregion Constructors

    #region Properties

    public SolidColorBrush BackgroundBrush { get; private set; }
    public BindingList<HistoryPageEntryViewModel> Entries { get; set; }
    public SolidColorBrush ForegroundBrush { get; private set; }
    public Guid Id { get; set; }
    public Brush LaneTextBrush { get; set; }
    public string Name { get; set; }
    public ICommand ReturnCommand { get; }

    #endregion Properties

    #region Methods

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
      this.Entries.Clear();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.Id = Guid.Parse(navigationContext.Parameters[LanePageViewModel.LaneIdParameter].ToString());

      this.SetDataModel(this.clipboardService.Lanes.Single(o => o.Id == this.Id));

      this.Load();
    }

    private async void Load()
    {
      var items = (await this.client.ClipboardGetlaneAsync(this.Id)).ToList();

      this.Entries = new BindingList<HistoryPageEntryViewModel>(items.Select(o => this.container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(o)).ToList());
    }

    protected override void OnReadingDataModel(LaneGetData data)
    {
      var backColor = (Color)ColorConverter.ConvertFromString(data.Color);
      this.BackgroundBrush = new SolidColorBrush(backColor);

      var foregroundColor = backColor.CalculateLuminance() > 0.6 ? Colors.Black : Colors.White;
      this.ForegroundBrush = new SolidColorBrush(foregroundColor);

      this.Name = data.Name;
    }

    #endregion Methods
  }
}