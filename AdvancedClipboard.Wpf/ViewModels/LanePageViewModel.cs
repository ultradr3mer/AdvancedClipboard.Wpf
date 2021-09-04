using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class LanePageViewModel : BaseViewModel<LaneGetData>, INavigationAware
  {
    #region Fields

    public const string LaneIdParameter = "Id";

    #endregion Fields

    #region Constructors

    public LanePageViewModel()
    {
    }

    #endregion Constructors

    #region Properties

    public SolidColorBrush BackgroundBrush { get; private set; }
    public BindingList<HistoryPageEntryViewModel> Entries { get; set; }
    public SolidColorBrush ForegroundBrush { get; private set; }
    public Guid Id { get; set; }
    public Brush LaneTextBrush { get; set; }
    public string Name { get; set; }

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
      var id = Guid.Parse(navigationContext.Parameters[LanePageViewModel.LaneIdParameter].ToString());
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