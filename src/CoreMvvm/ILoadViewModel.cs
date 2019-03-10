using System.Threading.Tasks;

namespace CoreMvvm
{
    public interface ILoadViewModel
    {
        bool IsBusy { get; set; }
        Task Load();
    }
}