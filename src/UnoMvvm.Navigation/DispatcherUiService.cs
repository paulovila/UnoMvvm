using System;

namespace UnoMvvm.Navigation
{
    public class DispatcherUiService : IDispatcherUiService
    {
#if __WASM__ || __IOS__
        public void Run(Action action)
        {
            action();
        }

#else
        public async void Run(Action action)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(action));
        }
#endif
    }
}