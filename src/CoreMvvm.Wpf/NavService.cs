using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CoreMvvm.Wpf
{
    public class NavService : NavServiceWpf
    {
        public NavService(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService) { }
        public static ContentControl GetContentControl(DependencyObject obj) => (ContentControl)obj.GetValue(ContentControlProperty);
        public static void SetContentControl(DependencyObject obj, ContentControl value) => obj.SetValue(ContentControlProperty, value);
        public static readonly DependencyProperty ContentControlProperty = DependencyProperty.RegisterAttached("ContentControl", typeof(ContentControl), typeof(NavService), new PropertyMetadata(null,
            (d, e) =>
            {
                if (!DesignerProperties.GetIsInDesignMode(d))
                {
                    OnContentControlChanged<NavService>(e.NewValue as ContentControl, true);
                }
            }
            ));
    }
}