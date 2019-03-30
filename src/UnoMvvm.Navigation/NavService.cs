using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoMvvm.Navigation
{
    public class NavService : NavServiceUwp
    {
        public NavService(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }

        public static ContentControl GetContentControl(DependencyObject obj) => (ContentControl)obj.GetValue(ContentControlProperty);
        public static void SetContentControl(DependencyObject obj, ContentControl value) => obj.SetValue(ContentControlProperty, value);
        public static readonly DependencyProperty ContentControlProperty = DependencyProperty.RegisterAttached("ContentControl", typeof(ContentControl), typeof(NavService),
            new PropertyMetadata(null, (d, e) =>
            {
                if (!DesignMode.DesignModeEnabled)
                {
                    OnContentControlChanged<NavService>(e.NewValue as ContentControl, true);
                }
            }));
    }
}