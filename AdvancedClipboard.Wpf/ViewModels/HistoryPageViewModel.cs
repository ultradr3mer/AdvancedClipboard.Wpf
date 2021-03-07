using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageViewModel : BaseViewModel
  {
    #region Fields

    private readonly IUnityContainer container;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public HistoryPageViewModel(IUnityContainer container, ClipboardService service)
    {
      this.container = container;
      this.service = service;

      this.RefreshCommand = new DelegateCommand(this.RefreshCommandExecute);

      this.Load();
    }

    #endregion Constructors

    #region Properties

    public BindingList<HistoryPageEntryViewModel> Entrys { get; set; }
    public DelegateCommand RefreshCommand { get; }

    #endregion Properties

    #region Methods

    protected virtual void Load()
    {
      this.service.ClipboardItems.ListChanged += this.ClipboardItemsListChanged;
      this.Entrys = new BindingList<HistoryPageEntryViewModel>(this.service.ClipboardItems.Select(o => this.container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(o)).ToList());
    }

    private void ClipboardItemsListChanged(object sender, ListChangedEventArgs e)
    {
      IList<ClipboardGetData> listSender = (IList<ClipboardGetData>)sender;

      if (e.ListChangedType == ListChangedType.ItemAdded)
      {
        ClipboardGetData addedData = listSender[e.NewIndex];
        HistoryPageEntryViewModel newEntry = this.container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(addedData);
        this.Entrys.Insert(e.NewIndex, newEntry);
      }
    }

    private void RefreshCommandExecute()
    {
      this.service.Refresh();
    }

    #endregion Methods
  }
}