using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation.Navigation;

internal class NavigationService : INavigationService
{
    private readonly ViewModelsFactoryService _viewModelFactoryService;

    public ApplicationBaseViewModel CurrentViewModel { get; protected set; } = null!;
    public NavigationService(ViewModelsFactoryService viewModelFactoryService) => _viewModelFactoryService = viewModelFactoryService;

    public void NavigateTo(Type applicationViewModelType) => TryCreateNewViewModel(applicationViewModelType);

    public void NavigateTo<T>() where T : ApplicationBaseViewModel => TryCreateNewViewModel(typeof(T));

    public void NavigateTo(ApplicationBaseViewModel applicationBaseViewModel) => SetNewViewModel(applicationBaseViewModel);

    public event EventHandler<ApplicationBaseViewModel>? ViewModelChanged;

    protected virtual void InvokeViewModelChanged() =>
        ViewModelChanged?.Invoke(this, CurrentViewModel);
    protected virtual void PostNavigateAction() { }

    private void TryCreateNewViewModel(Type viewModelTyp)
    {
        var viewModel = _viewModelFactoryService.CreateViewModel(viewModelTyp);
        if (viewModel is null)
        {
            throw new NavigationException($"Cannot create {viewModel}. Probably you haven't " +
                                          $"registered view models for they creating");
        }
        SetNewViewModel(viewModel);
    }
    private void SetNewViewModel(ApplicationBaseViewModel viewModel)
    {
        CurrentViewModel = viewModel;
        PostNavigateAction();
        InvokeViewModelChanged();
    }
}