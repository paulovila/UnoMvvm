using System;
using System.Threading.Tasks;

namespace UnoMvvm.Navigation
{
    public class DispatcherUiService : IDispatcherUiService
    {
#if __WASM__ || __IOS__
        public Task Run(Action action)
        {
            action();
            return  Task.CompletedTask;
        }

#else
        public async Task Run(Action action)
        {
           await  Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));
        }
#endif
    }
}