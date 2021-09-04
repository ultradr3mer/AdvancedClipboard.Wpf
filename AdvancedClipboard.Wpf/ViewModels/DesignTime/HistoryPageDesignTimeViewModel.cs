﻿using AdvancedClipboard.Wpf.Constants;
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

    public HistoryPageDesignTimeViewModel() : base(null, null, null)
    {
      this.Entries = new BindingList<HistoryPageEntryViewModel>()
      {
        this.CreateTextEntry("Erster Text"),
        this.CreateImageEntry("F451D57C64FC6140/clip_20210413_135842.png"),
        this.CreateFileEntry("3CDAB6DEE7BE995B/clip_20210414_133243.zip", "datei.zip"),
        this.CreateTextEntry("Zweiter Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text, ganz langer Text."),
        this.CreateTextEntry("Anderer Text"),
        this.CreateTextEntry("Noch ein Text"),
        this.CreateImageEntry("26C8C736070D7EFF/clip_20210316_213458.jpg"),
        this.CreateTextEntry("Dieser Text"),
        this.CreateTextEntry("Der andere Text")
      };

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

    private HistoryPageEntryViewModel CreateFileEntry(string url, string fileName)
    {
      return new HistoryPageEntryViewModel(null).GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.File, FileName = fileName });
    }

    private HistoryPageEntryViewModel CreateImageEntry(string url)
    {
      return new HistoryPageEntryViewModel(null).GetWithDataModel(new ClipboardGetData { FileContentUrl = url, ContentTypeId = ContentTypes.Image });
    }

    private HistoryPageEntryViewModel CreateTextEntry(string text)
    {
      return new HistoryPageEntryViewModel(null) { TextContent = text };
    }

    #endregion Constructors

    #region Methods

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected override async Task Load()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
      this.InputBoxVisibility = Visibility.Visible;
    }

    #endregion Methods
  }
}