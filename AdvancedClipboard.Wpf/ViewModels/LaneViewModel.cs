using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class LaneViewModel : BaseViewModel<LaneGetData>
  {
    private readonly IRegionManager regionManager;
    #region Constructors

    public LaneViewModel(IRegionManager regionManager)
    {
      this.OpenLaneCommand = new DelegateCommand(this.OpenLaneCommandExecute);
      this.regionManager = regionManager;
    }

    #endregion Constructors

    #region Properties

    public Brush BackgroundBrush { get; set; }

    public Brush ForegroundBrush { get; set; }

    public string Name { get; set; }

    public ICommand OpenLaneCommand { get; }

    public Guid Id { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(LaneGetData data)
    {
      var backColor = (Color)ColorConverter.ConvertFromString(data.Color);
      this.BackgroundBrush = new SolidColorBrush(backColor);

      var foregroundColor = backColor.CalculateLuminance() > 0.6 ? Colors.Black : Colors.White;
      this.ForegroundBrush = new SolidColorBrush(foregroundColor);

      this.Name = data.Name;
    }

    private void OpenLaneCommandExecute()
    {
      var navigationParameters = new NavigationParameters();
      navigationParameters.Add(LanePageViewModel.LaneIdParameter, this.Id);
      regionManager.RequestNavigate(App.RegionName, new Uri(nameof(LanePage) + navigationParameters.ToString(), UriKind.Relative));
    }

    #endregion Methods
  }
}