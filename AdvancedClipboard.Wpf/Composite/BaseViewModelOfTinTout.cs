
using AdvancedClipboard.Wpf.Extensions;

namespace AdvancedClipboard.Wpf.Composite
{
  public class BaseViewModel<Tin, Tout> : BaseViewModel where Tout : new()
  {
    #region Fields

    private Tin attachedDataModel;

    #endregion Fields

    #region Methods

    public void SetDataModel(Tin data)
    {
      this.CopyPropertiesFrom(data);
      this.attachedDataModel = data;
      this.OnReadingDataModel(data);
    }

    public Tout WriteToDataModel()
    {
      return new Tout().CopyPropertiesFrom(this.attachedDataModel).CopyPropertiesFrom(this);
    }

    protected virtual void OnReadingDataModel(Tin data)
    {
    }

    #endregion Methods
  }
}