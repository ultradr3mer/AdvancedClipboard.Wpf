﻿using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels
{
  internal class HistoryPageViewModel : BaseViewModel
  {
    #region Fields

    private const string ClosTextInputIcon = "";
    private const string OpenTextInputIcon = "";
    private readonly IUnityContainer container;
    private readonly ClipboardService service;

    #endregion Fields

    #region Constructors

    public HistoryPageViewModel(IUnityContainer container, ClipboardService service)
    {
      this.container = container;
      this.service = service;

      this.RefreshCommand = new DelegateCommand(this.RefreshCommandExecute);
      this.AddCommand = new DelegateCommand(this.AddCommandExecute);
      this.OpenCloseTextInputCommand = new DelegateCommand<bool?>(this.OpenCloseTextInputCommandExecute);
      this.AddTextInputCommand = new DelegateCommand(this.AddTextInputCommandExecute, this.AddTextInputCommandCanExecute);

      this.Load();

      this.OpenCloseTextInputContent = OpenTextInputIcon;

      this.PropertyChanged += this.HistoryPageViewModel_PropertyChanged;
    }

    #endregion Constructors

    #region Properties

    public ICommand AddCommand { get; }

    public DelegateCommand AddTextInputCommand { get; }

    public bool CanAddTextInput { get; set; }

    public BindingList<HistoryPageEntryViewModel> Entrys { get; set; }

    public Visibility InputBoxVisibility { get; set; }

    public ICommand OpenCloseTextInputCommand { get; }

    public string OpenCloseTextInputContent { get; set; }

    public ICommand RefreshCommand { get; }

    public string TextInput { get; set; }

    #endregion Properties

    #region Methods

    protected virtual void Load()
    {
      this.InputBoxVisibility = Visibility.Collapsed;
      this.service.ClipboardItems.ListChanged += this.ClipboardItemsListChanged;
      this.Entrys = new BindingList<HistoryPageEntryViewModel>(this.service.ClipboardItems.Select(o => this.container.Resolve<HistoryPageEntryViewModel>().GetWithDataModel(o)).ToList());
    }

    private void AddCommandExecute()
    {
      this.service.AddClipboardContent();
    }

    private bool AddTextInputCommandCanExecute()
    {
      return !string.IsNullOrWhiteSpace(this.TextInput);
    }

    private void AddTextInputCommandExecute()
    {
      this.service.AddClipboardContent(this.TextInput);
      this.TextInput = string.Empty;
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
      else if (e.ListChangedType == ListChangedType.ItemDeleted)
      {
        this.Entrys.RemoveAt(e.NewIndex);
      }
    }

    private void HistoryPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(TextInput))
      {
        this.AddTextInputCommand.RaiseCanExecuteChanged();
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

    private async void RefreshCommandExecute()
    {
      await this.service.Refresh();
    }

    #endregion Methods
  }
}