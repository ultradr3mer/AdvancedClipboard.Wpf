using AdvancedClipboard.Wpf.Services;
using Prism.Events;

namespace AdvancedClipboard.Wpf.Events
{
  internal class CredentialsChangedData
  {
    public CredentialsChangedData(SettingsService settingsService)
    {
      this.SettingsService = settingsService;
    }

    public SettingsService SettingsService { get; }
  }

  internal class CredentialsChangedEvent : PubSubEvent<CredentialsChangedData>
  {
  }
}