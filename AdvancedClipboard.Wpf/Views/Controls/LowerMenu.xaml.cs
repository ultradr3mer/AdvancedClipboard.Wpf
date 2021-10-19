using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdvancedClipboard.Wpf.Views.Controls
{
  /// <summary>
  /// Interaction logic for LowerMenu.xaml
  /// </summary>
  public partial class LowerMenu : UserControl
  {
    public static readonly DependencyProperty ButtonsBackgroundBrushProperty = DependencyProperty.Register(nameof(ButtonsBackgroundBrush), typeof(Brush), typeof(LowerMenu), new UIPropertyMetadata(null));

    public Brush ButtonsBackgroundBrush
    {
      get
      {
        return (Brush)GetValue(ButtonsBackgroundBrushProperty);
      }
      set
      {
        SetValue(ButtonsBackgroundBrushProperty, value);
      }
    }

    public static readonly DependencyProperty ButtonsForegroundBrushProperty = DependencyProperty.Register(nameof(ButtonsForegroundBrush), typeof(Brush), typeof(LowerMenu), new UIPropertyMetadata(null));

    public Brush ButtonsForegroundBrush
    {
      get
      {
        return (Brush)GetValue(ButtonsForegroundBrushProperty);
      }
      set
      {
        SetValue(ButtonsForegroundBrushProperty, value);
      }
    }

    public LowerMenu()
    {
      InitializeComponent();
    }
  }
}
