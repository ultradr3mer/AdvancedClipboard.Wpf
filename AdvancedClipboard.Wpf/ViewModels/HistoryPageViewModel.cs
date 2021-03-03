using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System.ComponentModel;
using System.Linq;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageViewModel : BaseViewModel
  {
    #region Fields

    private readonly Client client;
    private readonly IUnityContainer container;

    #endregion Fields

    #region Constructors

    public HistoryPageViewModel(IUnityContainer container, Client client)
    {
      this.container = container;
      this.client = client;

      this.LoadAsync();
    }

    #endregion Constructors

    #region Properties

    public BindingList<HistoryPageEntryViewModel> Entrys { get; set; }

    #endregion Properties

    #region Methods

    protected virtual async void LoadAsync()
    {
      var data = await client.ClipboardAsync();
      var vms = data.Select(o => container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(o)).ToList();
      this.Entrys = new BindingList<HistoryPageEntryViewModel>(vms);
    }

    #endregion Methods
  }
}