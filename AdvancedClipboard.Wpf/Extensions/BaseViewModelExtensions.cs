using AdvancedClipboard.Wpf.Composite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedClipboard.Wpf.Extensions
{
  public static class BaseViewModelExtensions
  {
    public static Tvm GetWithDataModel<Tvm, Tdata>(this Tvm value, Tdata data) where Tvm : BaseViewModel<Tdata>
    {
      value.SetDataModel(data);
      return value;
    }

    public static Tvm Create<Tvm, Tdata>(Tdata data) where Tvm : BaseViewModel<Tdata>, new()
    {
      var vm = new Tvm();
      vm.SetDataModel(data);
      return vm;
    }
  }
}
