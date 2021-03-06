using MahApps.Metro.Controls;
using ManagedWinapi;
using Prism.Regions;
using System.ComponentModel;
using System.Windows;
using Unity;

namespace AdvancedClipboard.Wpf.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    #region Fields

    private IUnityContainer container;
    private ClipboardNotifier notifier;

    #endregion Fields

    #region Constructors

    public MainWindow(IRegionManager regionManager, IUnityContainer container)
    {
      InitializeComponent();

      this.container = container;

      regionManager.RegisterViewWithRegion("MainRegion", () => container.Resolve<HistoryPage>());
    }

    #endregion Constructors

    #region Methods

    private void MetroWindowClosing(object sender, CancelEventArgs e)
    {
      this.notifier.Dispose();
    }

    private void MetroWindowLoaded(object sender, RoutedEventArgs e)
    {
      this.notifier = this.container.Resolve<ClipboardNotifier>();
    }

    #endregion Methods
  }
}