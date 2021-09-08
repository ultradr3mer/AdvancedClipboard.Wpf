using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unity;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  internal class HistoryPageDesignTimeViewModel : HistoryPageViewModel
  {
    #region Constructors

    public HistoryPageDesignTimeViewModel() : base(null, null, null, null)
    {
      this.Entries = new EntriesDesignTimeViewModel().Entries;

      this.Lanes = new BindingList<LaneViewModel>()
      {
        this.CreateLaneEntry("#FFDC143C", "Sql Abfragen"),
        this.CreateLaneEntry("#FF9400D3", "Status notizen"),
        this.CreateLaneEntry("#FF6495ED", "Helferlein"),
        this.CreateLaneEntry("#FFFFFFFF", "White"),
      };
    }

    private LaneViewModel CreateLaneEntry(string color, string name)
    {
      return new LaneViewModel(null).GetWithDataModel(new LaneGetData { Color = color, Name = name });
    }

    #endregion Constructors

    #region Methods

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected override async void Load()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
      this.InputBoxVisibility = Visibility.Visible;
    }

    #endregion Methods
  }
}