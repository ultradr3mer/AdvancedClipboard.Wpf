using AdvancedClipboard.Wpf.Extensions;

namespace AdvancedClipboard.Wpf.Composite
{
  public class BaseViewModel<T> : BaseViewModel
  {
    #region Fields

    private T attachedDataModel;

    #endregion Fields

    #region Methods

    public void SetDataModel(T data)
    {
      this.CopyPropertiesFrom(data);
      this.attachedDataModel = data;
      this.OnReadingDataModel(data);
    }

    public T WriteToDataModel()
    {
      return this.attachedDataModel.CopyPropertiesFrom(this);
    }

    protected virtual void OnReadingDataModel(T data)
    {
    }

    #endregion Methods
  }
}