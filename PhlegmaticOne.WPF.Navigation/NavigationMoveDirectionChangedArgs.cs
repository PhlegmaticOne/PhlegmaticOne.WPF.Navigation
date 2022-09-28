namespace PhlegmaticOne.WPF.Navigation;

public class NavigationMoveDirectionChangedArgs : EventArgs
{
    public NavigationMoveDirection NavigationMoveDirection { get; }
    public bool CanMove { get; }

    public NavigationMoveDirectionChangedArgs(NavigationMoveDirection navigationMoveDirection, bool canMove)
    {
        NavigationMoveDirection = navigationMoveDirection;
        CanMove = canMove;
    }
}