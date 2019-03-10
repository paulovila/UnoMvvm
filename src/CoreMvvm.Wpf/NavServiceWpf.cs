using System.Windows;
using System.Windows.Controls;

namespace CoreMvvm.Wpf
{
    public abstract class NavServiceWpf : NavServiceBase<ContentControl, FrameworkElement>
    {
        public NavServiceWpf(IDispatcherUiService dispatcherUiService) : base(dispatcherUiService)
        {
        }
        public override void SetDc(FrameworkElement view, object vm) => view.DataContext = vm;
        public override void SetContent(ContentControl content, FrameworkElement view) => content.Content = view;
    }
}