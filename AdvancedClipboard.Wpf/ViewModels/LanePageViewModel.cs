using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Interfaces;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class LanePageViewModel : BaseViewModel<LaneGetData>, INavigationAware, IEntryHostViewModel
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

    public BindingList<LaneEntryViewModel> Lanes { get; protected set; }

    public BindingList<HistoryPageEntryViewModel> Entries { get; protected set; }

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
      this.Lanes.Clear();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.Id = Guid.Parse(navigationContext.Parameters[LanePageViewModel.LaneIdParameter].ToString());

      this.Load();
    }

    private async void Load()
    {
      Task<ICollection<LaneGetData>> lanesTask = this.client.LaneGetAsync();
      Task<ClipboardPageGetData> entriesTask = this.client.ClipboardGetlanepageAsync(this.Id, 1);

      this.SetDataModel((await lanesTask).Single(o => o.Id == this.Id));
      this.Lanes = new BindingList<LaneEntryViewModel>((await lanesTask).Select(o => this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(o)).ToList());
      this.Entries = new BindingList<HistoryPageEntryViewModel>((await entriesTask).PageContent.Select(o => this.container.Resolve<HistoryPageEntryViewModel>().SetHost(this).GetWithDataModel(o)).ToList());
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