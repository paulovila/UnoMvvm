using System.Collections.Generic;

namespace UnoMvvm
{
    public interface IParametersViewModel : ILoadViewModel
    {
        IDictionary<string, object> Parameters { get; set; }
    }
}