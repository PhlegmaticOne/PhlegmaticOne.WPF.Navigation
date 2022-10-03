namespace PhlegmaticOne.WPF.Navigation.Exceptions;

/// <summary>
/// Base exception for all exception throwed in this assembly
/// </summary>
public class NavigationException : Exception
{
    public NavigationException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}