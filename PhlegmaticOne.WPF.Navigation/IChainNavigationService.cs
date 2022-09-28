namespace PhlegmaticOne.WPF.Navigation;

public interface IChainNavigationService : INavigationService
{
    event EventHandler<NavigationMoveDirectionChangedArgs> DirectionCanMoveChanged;
    void Move(NavigationMoveDirection direction);
    void Reset();
}