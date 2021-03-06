using AdvancedClipboard.Wpf.ViewModels;
using AdvancedClipboard.Wpf.Views;
using ManagedWinapi;
using Prism.Ioc;
using System.Windows;
using Unity;

namespace AdvancedClipboard.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
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
    }

    #endregion Methods
  }
}