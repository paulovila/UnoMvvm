using System;
using System.Threading.Tasks;

namespace UnoMvvm
{
    public abstract class NavServiceBase<TCt, TFe> : INavService where TFe : class
    {
        public Action<Exception> NavigationFailed { get; set; }
        private TCt _content;
        private TFe _temporaryNavigation;
        private readonly IDispatcherUiService _dispatcherUiService;

        public NavServiceBase(IDispatcherUiService dispatcherUiService) => _dispatcherUiService = dispatcherUiService;

        public async Task Navigate<TVm>() where TVm : IViewModel
        {
            var view = ViewModelLocationProvider.NewViewFromVm<TVm>() as TFe;
            var vm = ViewModelLocationProvider.ViewModelFactory(typeof(TVm));
            SetDc(view, vm);
            await PrepareNavigation(view);
            if (vm is ILoadViewModel vmLoad)
                await TryLoad(vmLoad);
        }

        public async Task Navigate<TVm, TP>(TP parameters)
            where TVm : IViewModel

        {
            var view = ViewModelLocationProvider.NewViewFromVm<TVm>() as TFe;
            var vm = ViewModelLocationProvider.ViewModelFactory(typeof(TVm));

            if (vm is IParameterViewModel<TP> vmPar)
            {
                vmPar.Parameter = parameters;
            }
            SetDc(view, vm);
            await PrepareNavigation(view);
            if (vm is ILoadViewModel vmLoad)
                await TryLoad(vmLoad);
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

        private async Task PrepareNavigation(TFe view)
        {
            if (_content == null)
            {
                _temporaryNavigation = view;
            }
            else
            {
                _temporaryNavigation = null;
                await _dispatcherUiService.Run(() => SetContent(_content, view));
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

        public void Clear() => _dispatcherUiService.Run(() => SetContent(_content, null));
    }
}