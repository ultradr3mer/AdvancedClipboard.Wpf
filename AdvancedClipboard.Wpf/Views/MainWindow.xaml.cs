using MahApps.Metro.Controls;
using Prism.Regions;
using Unity;

namespace AdvancedClipboard.Wpf.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    #region Constructors

    public MainWindow(IRegionManager regionManager, IUnityContainer container)
    {
      InitializeComponent();

      regionManager.RegisterViewWithRegion("MainRegion", () => container.Resolve<HistoryPage>());
    }

    #endregion Constructors
  }
}