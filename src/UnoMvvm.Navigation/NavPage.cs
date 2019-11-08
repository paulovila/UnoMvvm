using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UnoMvvm.Navigation
{
    public class NavPage<T> : Page where T : FrameworkElement
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if __IOS__
            Content = (UIKit.UIView)e.Parameter;
#else
            Content = (UIElement)e.Parameter;
#endif
            base.OnNavigatedFrom(e);
        }
    }
}
