namespace PhlegmaticOne.WPF.Navigation.Sample.Contracts.Results;

public class SelectResult<T> : ResultBase
{
    public IList<T> Result { get; init; }
}
