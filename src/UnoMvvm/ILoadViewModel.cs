using System.Threading.Tasks;

namespace UnoMvvm
{
    public interface ILoadViewModel
    {
        bool IsBusy { get; set; }
        Task Load();
    }
}