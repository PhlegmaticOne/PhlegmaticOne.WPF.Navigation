namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Contract for chain navigation
/// </summary>
public interface IChainNavigationService : INavigationService
{
    /// <summary>
    /// Raises when navigation cannot navigate forward or back
    /// </summary>
    event EventHandler<NavigationMoveDirectionChangedArgs> DirectionCanMoveChanged;
    /// <summary>
    /// Moves to next or previous ApplicationViewModel in chain
    /// </summary>
    /// <param name="direction">Direction of moving</param>
    void Move(NavigationMoveDirection direction);
    /// <summary>
    /// Clears all ApplicationViewModels in chain. After calling this method you should navigate to another ApplicationViewModel  
    /// </summary>
    void Reset();
}