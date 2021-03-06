using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System.Collections.Generic;
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

    public HistoryPageViewModel(IUnityContainer container, ClipboardService service)
    {
      service.ClipboardItems.ListChanged += this.ClipboardItemsListChanged;
      this.Entrys = new BindingList<HistoryPageEntryViewModel>(service.ClipboardItems.Select(o => container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(o)).ToList());
      this.container = container;
    }

    #endregion Constructors

    #region Properties

    public BindingList<HistoryPageEntryViewModel> Entrys { get; set; }

    #endregion Properties

    #region Methods

    private void ClipboardItemsListChanged(object sender, ListChangedEventArgs e)
    {
      var listSender = (IList<ClipboardGetData>)sender;

      if(e.ListChangedType == ListChangedType.ItemAdded)
      {
        var addedData = listSender[e.NewIndex];
        var newEntry = container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(addedData);
        this.Entrys.Insert(e.NewIndex, newEntry);
      }
    }

    #endregion Methods
  }
}