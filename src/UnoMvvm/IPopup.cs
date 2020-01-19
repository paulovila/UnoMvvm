using System.Threading.Tasks;

namespace UnoMvvm
{
    public interface IPopup
    {
        Task<bool?> ShowMessageAsync(string title, string message);
        Task PopAsync<TVM>();
        Task PopAsync<TVM, T>(T parameters) where TVM : IViewModel;
        Task CloseDialog<TVM>();
    }
}