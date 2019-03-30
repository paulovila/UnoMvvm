using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoMvvm.Navigation
{
    public abstract class NavServiceUwp : NavServiceBase<ContentControl, FrameworkElement>
    {
        public NavServiceUwp(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }
        public override void SetDc(FrameworkElement view, object vm) => view.DataContext = vm;
        public override void SetContent(ContentControl content, FrameworkElement view) => content.Content = view;
    }
}