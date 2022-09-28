using PhlegmaticOne.WPF.Navigation.Navigation;

namespace PhlegmaticOne.WPF.Navigation.ChainNavigation;

internal class ChainNavigationService : NavigationService, IChainNavigationService
{
    private readonly NavigationHistory _navigationHistory;
    internal ChainNavigationService(IViewModelFactoryService viewModelFactoryService) : base(viewModelFactoryService) =>
        _navigationHistory = new NavigationHistory();

    public event EventHandler<NavigationMoveDirectionChangedArgs>? DirectionCanMoveChanged;

    public void Move(NavigationMoveDirection direction)
    {
        if (_navigationHistory.Current is not null)
        {
            _navigationHistory.Move(direction);
            CurrentViewModel = _navigationHistory.Current.ViewModel;
            InvokeViewModelChanged();
        }
    }
    public void Reset()
    {
        _navigationHistory.Reset();
    }

    protected override void PostNavigateAction() => _navigationHistory.Add(CurrentViewModel);

    protected override void InvokeViewModelChanged()
    {
        base.InvokeViewModelChanged();
        InvokeDirectionCanMoveChanged(NavigationMoveDirection.Forward, _navigationHistory.CanMoveForward);
        InvokeDirectionCanMoveChanged(NavigationMoveDirection.Back, _navigationHistory.CanMoveBack);
    }

    protected void InvokeDirectionCanMoveChanged(NavigationMoveDirection navigationMoveDirection, bool canMove) =>
        DirectionCanMoveChanged?.Invoke(this, new(navigationMoveDirection, canMove));
}