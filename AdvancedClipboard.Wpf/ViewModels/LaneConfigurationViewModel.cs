using AdvancedClipboard.Wpf.Composite;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.ViewModels
{
  public class LaneConfigurationViewModel : BaseViewModel<LaneGetData>
  {
    #region Fields

    private readonly ConfigurationPageViewModel configurationViewModel;

    #endregion Fields

    #region Constructors

    public LaneConfigurationViewModel(ConfigurationPageViewModel configurationViewModel)
    {
      this.DeleteCommand = new DelegateCommand(this.DeleteCommandExecute);
      this.configurationViewModel = configurationViewModel;

      this.PropertyChanged += this.LaneConfigurationViewModel_PropertyChanged;
    }

    private void LaneConfigurationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if(e.PropertyName == nameof(Color) || e.PropertyName == nameof(Name))
      {
        this.IsEdited = true;
      }
    }

    #endregion Constructors

    #region Properties

    public Color Color { get; set; }

    public bool IsEdited { get; set; }

    public ICommand DeleteCommand { get; }

    public Guid Id { get; set; }

    public string Name { get; set; }

    #endregion Properties

    #region Methods

    protected override void OnReadingDataModel(LaneGetData data)
    {
      this.Color = (Color)ColorConverter.ConvertFromString(data.Color);

      this.IsEdited = false;
    }

    private void DeleteCommandExecute()
    {
      this.configurationViewModel.Lanes.Remove(this);
      this.configurationViewModel.DeletedLanes.Add(this);
    }

    #endregion Methods
  }
}