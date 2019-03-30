using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoMvvm.Navigation
{
    public class NavFrameUwp : NavServiceBase<Frame, FrameworkElement>
    {
        public NavFrameUwp(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }

        public override void SetContent(Frame content, FrameworkElement view)
        {
            content.Navigate(typeof(NavPage<>).MakeGenericType(view.GetType()), view);
        }

        public override void SetDc(FrameworkElement view, object vm)
        {
            view.DataContext = vm;
        }
    }
}
