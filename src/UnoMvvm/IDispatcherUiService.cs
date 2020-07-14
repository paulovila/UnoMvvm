using System;
using System.Threading.Tasks;

namespace UnoMvvm
{
    public interface IDispatcherUiService
    {
        Task Run(Action action);
    }
}