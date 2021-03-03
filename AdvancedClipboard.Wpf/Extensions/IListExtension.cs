using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedClipboard.Wpf.Extensions
{
  static class IListExtension
  {
    public static void AddRangeSequential<T>(this IList<T> self, IEnumerable<T> otherList)
    {
      foreach (var item in otherList)
      {
        self.Add(item);
      }
    }
  }
}
