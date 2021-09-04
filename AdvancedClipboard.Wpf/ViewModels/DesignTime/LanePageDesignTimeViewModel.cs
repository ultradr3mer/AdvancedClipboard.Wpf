using AdvancedClipboard.Wpf.Constants;
using AdvancedClipboard.Wpf.Extensions;
using AdvancedClipboard.Wpf.Services;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AdvancedClipboard.Wpf.ViewModels.DesignTime
{
  internal class LanePageDesignTimeViewModel : LanePageViewModel
  {
    #region Constructors

    public LanePageDesignTimeViewModel() : base(null,null,null,null)
    {
      this.Entries = new EntriesDesignTimeViewModel().Entries;
    
      this.SetDataModel(this.CreateLaneEntry("#FFDC143C", "Sql Abfragen"));
    }

    #endregion Constructors

    #region Methods

    private LaneGetData CreateLaneEntry(string color, string name)
    {
      return new LaneGetData { Color = color, Name = name };
    }

    #endregion Methods
  }
}