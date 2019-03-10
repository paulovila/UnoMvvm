using System;

namespace CoreMvvm
{
    public interface IDispatcherUiService
    {
        void Run(Action action);
    }
}