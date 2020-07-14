using System;
using System.Threading.Tasks;

namespace UnoMvvm
{
    public interface INavService
    {
        Task Navigate<T>() where T : IViewModel;
        Task Navigate<T, TP>(TP parameters) where T : IViewModel;
        void Clear();
        Action<Exception> NavigationFailed { get; set; }
    }
}