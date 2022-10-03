using PhlegmaticOne.WPF.Core.Commands;
using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Sample.ViewModels;

public class NavigationViewModel : ApplicationBaseViewModel, IDisposable
{
    private readonly IChainNavigationService _chainNavigationService;
    public NavigationViewModel(IChainNavigationService chainNavigationService)
    {
        _chainNavigationService = chainNavigationService;
        _chainNavigationService.DirectionCanMoveChanged += ChainNavigationServiceOnDirectionCanMoveChanged;
        _chainNavigationService.ViewModelChanged += ChainNavigationServiceOnViewModelChanged;

        MoveCommand = RelayCommandFactory
            .CreateRequiredParameterCommand<NavigationMoveDirection>(Move, _ => true);

        NavigateCommand = RelayCommandFactory
            .CreateRequiredParameterCommand<Type>(Navigate, _ => true);

        ResetCommand = RelayCommandFactory.CreateCommand(Reset, _ => true);

        NavigateDefault();
    }
    public bool CanMoveBack { get; private set; }
    public bool CanMoveForward { get; private set; }
    public ApplicationBaseViewModel CurrentViewModel { get; private set; } = null!;
    public IRelayCommand NavigateCommand { get; }
    public IRelayCommand MoveCommand { get; }
    public IRelayCommand ResetCommand { get; }

    private void Move(NavigationMoveDirection navigationMoveDirection)
    {
        _chainNavigationService.Move(navigationMoveDirection);
    }

    private void Navigate(Type parameter)
    {
        _chainNavigationService.NavigateTo(parameter);
    }
    private void Reset(object? _)
    {
        _chainNavigationService.Reset();
        NavigateDefault();
    }
    private void NavigateDefault()
    {
        Navigate(typeof(HomeViewModel));
    }
    private void ChainNavigationServiceOnViewModelChanged(object? sender, ApplicationBaseViewModel e)
    {
        CurrentViewModel = e;
    }

    private void ChainNavigationServiceOnDirectionCanMoveChanged(object? sender, NavigationMoveDirectionChangedArgs e)
    {
        switch (e.NavigationMoveDirection)
        {
            case NavigationMoveDirection.Forward:
            {
                CanMoveForward = e.CanMove;
                break;
            }
            case NavigationMoveDirection.Back:
            {
                CanMoveBack = e.CanMove;
                break;
            }
        }
    }

    public void Dispose()
    {
        _chainNavigationService.DirectionCanMoveChanged -= ChainNavigationServiceOnDirectionCanMoveChanged;
        _chainNavigationService.ViewModelChanged -= ChainNavigationServiceOnViewModelChanged;
    }
}