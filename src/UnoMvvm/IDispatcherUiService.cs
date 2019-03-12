using System;

namespace UnoMvvm
{
    public interface IDispatcherUiService
    {
        void Run(Action action);
    }
}