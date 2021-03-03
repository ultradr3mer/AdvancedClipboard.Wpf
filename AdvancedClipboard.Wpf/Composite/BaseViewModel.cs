using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdvancedClipboard.Wpf.Composite
{
  public class BaseViewModel : INotifyPropertyChanged
  {
    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    public bool IsBusy { get; set; }

    public string Title { get; set; }

    #endregion Properties

    #region Methods

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChangedEventHandler changed = PropertyChanged;
      if (changed == null)
      {
        return;
      }

      changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion Methods
  }
}