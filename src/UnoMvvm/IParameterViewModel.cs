namespace UnoMvvm
{
    public interface IParameterViewModel<T> : ILoadViewModel
    {
        T Parameter { get; set; }
    }
}