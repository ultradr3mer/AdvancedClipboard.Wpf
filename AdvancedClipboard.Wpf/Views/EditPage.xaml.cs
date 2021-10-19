using AdvancedClipboard.Wpf.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdvancedClipboard.Wpf.Views
{
  /// <summary>
  /// Interaction logic for EditPage.xaml
  /// </summary>
  public partial class EditPage : UserControl
  {
    #region Constructors

    public EditPage()
    {
      InitializeComponent();
    }

    #endregion Constructors

    #region Methods

    private void linkTextboxMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      ((EditPageViewModel)this.DataContext).OpenShareUrl.Execute(null);
    }

    #endregion Methods
  }
}