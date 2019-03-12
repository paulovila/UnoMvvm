using System;

namespace UnoMvvm
{
    public interface INavService
    {
        void Navigate<T>() where T : IViewModel;
        void Navigate<T, TP>(TP parameters) where TP : INavigationParameters where T : IViewModel;
        Action<Exception> NavigationFailed { get; set; }
    }
}