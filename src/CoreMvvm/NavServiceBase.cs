using System;
using System.Threading.Tasks;

namespace CoreMvvm
{
    public abstract class NavServiceBase<TCt, TFe> : INavService where TFe : class
    {
        public Action<Exception> NavigationFailed { get; set; }
        private TCt _content;
        private TFe _temporaryNavigation;
        private readonly IDispatcherUiService _dispatcherUiService;

        public NavServiceBase(IDispatcherUiService dispatcherUiService)
        {
            _dispatcherUiService = dispatcherUiService;
        }

        public async void Navigate<TVM>() where TVM : IViewModel
        {
            var view = ViewModelLocationProvider.NewViewFromVm<TVM>() as TFe;
            var vm = ViewModelLocationProvider.ViewModelFactory(typeof(TVM));
            SetDc(view, vm);
            PrepareNavigation(view);
            await TryLoad(vm as ILoadViewModel);
        }

        public async void Navigate<TVM, TP>(TP parameters)
            where TVM : IViewModel
            where TP : INavigationParameters
        {
            var view = ViewModelLocationProvider.NewViewFromVm<TVM>() as TFe;
            var vm = ViewModelLocationProvider.ViewModelFactory(typeof(TVM));

            var vmPar = vm as IParametersViewModel;
            if (vmPar != null)
            {
                vmPar.Parameters = parameters;
            }
            SetDc(view, vm);
            PrepareNavigation(view);
            await TryLoad(vm as ILoadViewModel);
        }

        public abstract void SetDc(TFe view, object vm);
        public abstract void SetContent(TCt content, TFe view);
        private async Task TryLoad(ILoadViewModel vm)
        {
            try
            {
                await vm.Load();
            }
            catch (Exception ex)
            {
                NavigationFailed?.Invoke(ex);
            }
        }

        private void PrepareNavigation(TFe view)
        {
            if (_content == null)
            {
                _temporaryNavigation = view;
            }
            else
            {
                _temporaryNavigation = null;
                _dispatcherUiService.Run(() => SetContent(_content, view));
            }
        }

        public static void OnContentControlChanged<TNav>(TCt content, bool isDefault = false) where TNav : NavServiceBase<TCt, TFe>
        {
            var navType = isDefault ? typeof(INavService) : typeof(TNav);
            var navService = ViewModelLocationProvider.ViewModelFactory(navType) as TNav;
            if (navService._content == null && content != null)
            {
                navService._content = content;
                navService.SetContent(navService._content, navService._temporaryNavigation);
                navService._temporaryNavigation = null;
            }
        }
    }
}