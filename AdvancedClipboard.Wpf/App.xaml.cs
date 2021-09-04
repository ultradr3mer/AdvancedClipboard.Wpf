using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.ViewModels;
using AdvancedClipboard.Wpf.Views;
using ManagedWinapi;
using Prism.Ioc;
using System.Net.Http;
using System.Windows;
using Unity;

namespace AdvancedClipboard.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    #region Fields

    public const string RegionName = "MainRegion";

    #endregion Fields

    #region Methods

    protected override Window CreateShell()
    {
      var mainWindow = Container.Resolve<MainWindow>();
      return mainWindow;
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<MainWindowViewModel>();
      containerRegistry.RegisterSingleton<ClipboardNotifier>();
      containerRegistry.RegisterSingleton<Client>();
      containerRegistry.RegisterSingleton<ClipboardService>();
      containerRegistry.RegisterSingleton<HistoryPageViewModel>();

      var container = this.Container.Resolve<IUnityContainer>();
      container.RegisterType<HttpClient, CustomHttpClient>();
    }

    #endregion Methods
  }
}