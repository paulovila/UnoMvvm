using System;
using System.Threading.Tasks;

namespace UnoMvvm
{
    public class NavServiceDesign : INavService
    {
        public Task Navigate<T>() where T : IViewModel => Task.CompletedTask;
        public Task Navigate<T, TP>(TP parameters) where T : IViewModel => Task.CompletedTask;
        public Action<Exception> NavigationFailed { get; set; } = _ => { };
        public void Clear() { }
    }
}