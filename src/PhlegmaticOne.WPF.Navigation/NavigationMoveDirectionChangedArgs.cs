namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Event arguments for NavigationMoveDirectionChanged event
/// </summary>
public class NavigationMoveDirectionChangedArgs : EventArgs
{
    /// <summary>
    /// Direction where navigation has moved
    /// </summary>
    public NavigationMoveDirection NavigationMoveDirection { get; }
    /// <summary>
    /// Indicates whether navigation can move to this direction further 
    /// </summary>
    public bool CanMove { get; }

    public NavigationMoveDirectionChangedArgs(NavigationMoveDirection navigationMoveDirection, bool canMove)
    {
        NavigationMoveDirection = navigationMoveDirection;
        CanMove = canMove;
    }
}