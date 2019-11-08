using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoMvvm.Navigation
{
    public class NavFrame : NavFrameUwp
    {
        public NavFrame(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }

        public static Frame GetFrame(DependencyObject obj) => (Frame)obj.GetValue(FrameProperty);
        public static void SetFrame(DependencyObject obj, Frame value) => obj.SetValue(FrameProperty, value);
        public static readonly DependencyProperty FrameProperty = DependencyProperty.RegisterAttached("Frame", typeof(Frame), typeof(NavFrame),
            new PropertyMetadata(null, (d, e) =>
            {
                if (!DesignMode.DesignModeEnabled)
                {
                    OnContentControlChanged<NavFrame>(e.NewValue as Frame, true);
                }
            }));
    }
}
