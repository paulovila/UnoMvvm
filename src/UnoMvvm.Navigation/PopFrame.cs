using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UnoMvvm.Navigation
{
    public class PopFrame : NavFrameUwp, IPopNavService
    {
        public PopFrame(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }

        public static Frame GetFrame(DependencyObject obj) => (Frame)obj.GetValue(FrameProperty);
        public static void SetFrame(DependencyObject obj, Frame value) => obj.SetValue(FrameProperty, value);
        public static readonly DependencyProperty FrameProperty = DependencyProperty.RegisterAttached("Frame", typeof(Frame), typeof(PopFrame),
            new PropertyMetadata(null, (d, e) =>
            {
                if (!DesignMode.DesignModeEnabled)
                {
                    OnContentControlChanged<PopFrame>(e.NewValue as Frame, false);
                }
            }));
    }
}