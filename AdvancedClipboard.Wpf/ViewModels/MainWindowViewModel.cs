using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Services;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class MainWindowViewModel : BaseViewModel
  {
    #region Constructors

    public MainWindowViewModel(IUnityContainer unityContainer)
    {
      var client = new Client(new CustomHttpClient("ClaraOriginal", ""));
      unityContainer.RegisterInstance(client);
    }

    #endregion Constructors
  }
}