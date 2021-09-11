using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System.ComponentModel;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  public class ConfigurationPageDesignTimeViewModel : ConfigurationPageViewModel
  {
    #region Constructors

    public ConfigurationPageDesignTimeViewModel() : base(null, null, null, null)
    {
      this.Lanes = new BindingList<LaneConfigurationViewModel>()
      {
        this.CreateLaneEntry("#FFDC143C", "Sql Abfragen"),
        this.CreateLaneEntry("#FF9400D3", "Status notizen"),
        this.CreateLaneEntry("#FF6495ED", "Helferlein"),
        this.CreateLaneEntry("#FFFFFFFF", "White"),
      };
    }

    #endregion Constructors

    #region Methods

    private LaneConfigurationViewModel CreateLaneEntry(string color, string name)
    {
      return new LaneConfigurationViewModel(null).GetWithDataModel(new LaneGetData { Color = color, Name = name });
    }

    #endregion Methods
  }
}