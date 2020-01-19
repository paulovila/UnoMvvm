using System;
using System.Collections.Generic;

namespace UnoMvvm
{
    public static class ViewModelLocationProvider
    {
        public static readonly Dictionary<Type, Type> V_VmDictionary = new Dictionary<Type, Type>();
        public static readonly Dictionary<Type, Type> Vm_vDictionary = new Dictionary<Type, Type>();
        public static Func<Type, object> ViewModelFactory;
        public static void Register<TV, TVM>()
        {
            V_VmDictionary[typeof(TV)] = typeof(TVM);
            Vm_vDictionary[typeof(TVM)] = typeof(TV);
        }
        public static object NewViewFromVm<TVM>() => Activator.CreateInstance(Vm_vDictionary[typeof(TVM)]);
    }
}