using AdvancedClipboard.Wpf.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdvancedClipboard.Wpf.Views.Controls
{
  /// <summary>
  /// Interaction logic for Entry.xaml
  /// </summary>
  public partial class Entry : UserControl
  {
    #region Constructors

    public Entry()
    {
      this.InitializeComponent();
    }

    #endregion Constructors

    #region Properties

    private HistoryPageEntryViewModel ViewModel
    {
      get => (HistoryPageEntryViewModel)this.DataContext;
    }

    #endregion Properties

    #region Methods

    private FlowDocument ConvertToFlowDocument(string text)
    {
      FlowDocument flowDocument = new FlowDocument();

      if (text == null)
      {
        return flowDocument;
      }

      Regex regex = new Regex(@"(https?:\/\/[^\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
      System.Collections.Generic.List<string> matches = regex.Matches(text).Cast<Match>().Select(m => m.Value).ToList();

      Paragraph paragraph = new Paragraph();
      flowDocument.Blocks.Add(paragraph);

      foreach (string segment in regex.Split(text))
      {
        if (matches.Contains(segment))
        {
          Hyperlink hyperlink = new Hyperlink(new Run(segment) { FontSize = 14 })
          {
            NavigateUri = new Uri(segment),
          };
          hyperlink.RequestNavigate += (sender, args) => Process.Start(segment);

          paragraph.Inlines.Add(hyperlink);
        }
        else
        {
          paragraph.Inlines.Add(new Run(segment) { Foreground = new SolidColorBrush(Colors.White), FontSize = 14 });
        }
      }

      return flowDocument;
    }

    private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(HistoryPageEntryViewModel.TextContent))
      {
        this.UpdateText();
      }
    }

    private void TextContent_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      this.ViewModel.LoadIntoClipboardCommand.Execute(null);
    }

    private void UpdateText()
    {
      string text = this.ViewModel.TextContent;
      this.TextContent.Document = this.ConvertToFlowDocument(text);
    }

    private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.ViewModel.PropertyChanged += this.Entry_PropertyChanged;
      this.UpdateText();
    }

    #endregion Methods
  }
}