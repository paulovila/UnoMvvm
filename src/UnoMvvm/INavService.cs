using System;

namespace UnoMvvm
{
    public interface INavService
    {
        void Navigate<T>() where T : IViewModel;
        void Navigate<T, TP>(TP parameters) where T : IViewModel;
        void Clear();
        Action<Exception> NavigationFailed { get; set; }
    }
}