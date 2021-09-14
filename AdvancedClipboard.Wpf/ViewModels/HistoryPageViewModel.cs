using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Interfaces;
using AdvancedClipboard.Wpf.Services;
using AdvancedClipboard.Wpf.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageViewModel : BaseViewModel, INavigationAware, IEntryHostViewModel
  {
    #region Fields

    private const string ClosTextInputIcon = "";
    private const string OpenTextInputIcon = "";
    private readonly Client client;
    private readonly IRegionManager regionManager;
    private readonly IUnityContainer container;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public HistoryPageViewModel(IUnityContainer container, ClipboardService service, Client client, IRegionManager regionManager)
    {
      this.container = container;
      this.service = service;
      this.client = client;
      this.regionManager = regionManager;
      this.RefreshCommand = new DelegateCommand(this.RefreshCommandExecute);
      this.AddCommand = new DelegateCommand(this.AddCommandExecute);
      this.OpenCloseTextInputCommand = new DelegateCommand<bool?>(this.OpenCloseTextInputCommandExecute);
      this.AddTextInputCommand = new DelegateCommand(this.AddTextInputCommandExecute, this.AddTextInputCommandCanExecute);
      this.OpenConfigurationCommand = new DelegateCommand(this.OpenConfigurationCommandExecute);

      this.OpenCloseTextInputContent = OpenTextInputIcon;

      this.PropertyChanged += this.HistoryPageViewModel_PropertyChanged;
    }

    private void OpenConfigurationCommandExecute()
    {
      regionManager.RequestNavigate(App.RegionName, new Uri(nameof(ConfigurationPage), UriKind.Relative));
    }

    #endregion Constructors

    #region Properties

    public ICommand AddCommand { get; }

    public DelegateCommand AddTextInputCommand { get; }
    
    public bool CanAddTextInput { get; set; }
    
    public BindingList<HistoryPageEntryViewModel> Entries { get; protected set; }
    
    public Visibility InputBoxVisibility { get; set; }
    
    public ICommand OpenCloseTextInputCommand { get; }
    
    public string OpenCloseTextInputContent { get; set; }
    
    public ICommand RefreshCommand { get; }
    
    public string TextInput { get; set; }
    
    public BindingList<LaneEntryViewModel> Lanes { get; protected set; }
    
    public ICommand OpenConfigurationCommand { get; }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
      this.Lanes.Clear();
      this.Entries.Clear();
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
      this.Load();
    }

    #endregion Properties

    #region Methods

    protected virtual async void Load()
    {
      this.InputBoxVisibility = Visibility.Collapsed;

      Task<ICollection<LaneGetData>> lanesTask = this.client.LaneGetAsync();
      Task<ClipboardPageGetData> entriesTask = this.client.ClipboardGetpageAsync(1);

      this.Lanes = new BindingList<LaneEntryViewModel>((await lanesTask).Select(o => this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(o)).ToList());
      this.Entries = new BindingList<HistoryPageEntryViewModel>((await entriesTask).PageContent.Select(CreateEntryViewModel).ToList());
    }

    private HistoryPageEntryViewModel CreateEntryViewModel(ClipboardGetData o)
    {
      return this.container.Resolve<HistoryPageEntryViewModel>().SetHost(this).GetWithDataModel(o);
    }

    private async void AddCommandExecute()
    {
      var newItems = await this.service.AddClipboardContent();
      foreach (var item in newItems)
      {
        this.Entries.Insert(0, this.CreateEntryViewModel(item));
      }
    }

    private bool AddTextInputCommandCanExecute()
    {
      return !string.IsNullOrWhiteSpace(this.TextInput);
    }

    private async void AddTextInputCommandExecute()
    {
      var newItem = await this.service.AddClipboardContent(this.TextInput);
      this.Entries.Insert(0, this.CreateEntryViewModel(newItem));
      this.TextInput = string.Empty;
    }

    private void ClipboardItemsListChanged(object sender, ListChangedEventArgs e)
    {
      IList<ClipboardGetData> listSender = (IList<ClipboardGetData>)sender;

      if (e.ListChangedType == ListChangedType.ItemAdded)
      {
        ClipboardGetData addedData = listSender[e.NewIndex];
        HistoryPageEntryViewModel newEntry = this.container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(addedData);
        this.Entries.Insert(e.NewIndex, newEntry);
      }
      else if (e.ListChangedType == ListChangedType.ItemDeleted)
      {
        this.Entries.RemoveAt(e.NewIndex);
      }
      else if (e.ListChangedType == ListChangedType.Reset)
      {
        if (listSender.Count == 0)
        {
          this.Entries = new BindingList<HistoryPageEntryViewModel>();
        }
      }
    }

    private void HistoryPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(TextInput))
      {
        this.AddTextInputCommand.RaiseCanExecuteChanged();
      }
    }

    private void LaneItemsListChanged(object sender, ListChangedEventArgs e)
    {
      IList<LaneGetData> listSender = (IList<LaneGetData>)sender;

      if (e.ListChangedType == ListChangedType.ItemAdded)
      {
        LaneGetData addedData = listSender[e.NewIndex];
        LaneEntryViewModel newEntry = this.container.Resolve<LaneEntryViewModel>().GetWithDataModel(addedData);
        this.Lanes.Insert(e.NewIndex, newEntry);
      }
      else if (e.ListChangedType == ListChangedType.ItemDeleted)
      {
        this.Lanes.RemoveAt(e.NewIndex);
      }
      else if (e.ListChangedType == ListChangedType.Reset)
      {
        if (listSender.Count == 0)
        {
          this.Lanes = new BindingList<LaneEntryViewModel>();
        }
      }
    }

    private void OpenCloseTextInputCommandExecute(bool? isChecked)
    {
      if (isChecked == true)
      {
        this.InputBoxVisibility = Visibility.Visible;
        this.OpenCloseTextInputContent = ClosTextInputIcon;
      }
      else
      {
        this.InputBoxVisibility = Visibility.Collapsed;
        this.OpenCloseTextInputContent = OpenTextInputIcon;
      }
    }

    private void RefreshCommandExecute()
    {
      this.Load();
    }

    #endregion Methods
  }
}