using Prism.Events;

namespace AdvancedClipboard.Wpf.Events
{
  internal class ClipboardChangedData
  {
  }

  internal class ClipboardChangedEvent : PubSubEvent<ClipboardChangedData>
  {
  }
}