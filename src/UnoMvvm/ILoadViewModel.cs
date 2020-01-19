using System.Threading.Tasks;

namespace UnoMvvm
{
    public interface ILoadViewModel: IViewModel
    {
        bool IsBusy { get; set; }
        Task Load();
    }
}