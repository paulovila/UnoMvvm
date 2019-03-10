using System.Collections.Generic;

namespace CoreMvvm
{
    public interface IParametersViewModel : ILoadViewModel
    {
        IDictionary<string, object> Parameters { get; set; }
    }
}