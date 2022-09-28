namespace PhlegmaticOne.WPF.Navigation.Exceptions;

public class NavigationException : Exception
{
    public NavigationException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
}