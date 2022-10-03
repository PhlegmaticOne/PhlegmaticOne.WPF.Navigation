using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation.EntityContainingViewModels;

internal class EntityContainingViewModelNavigation<TFrom, TTo> : IEntityContainingViewModelNavigation<TFrom, TTo>
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
    private readonly INavigationFactory<TFrom, TTo> _navigationFactory;
    private readonly INavigationService _navigationService;
    private readonly ViewModelsFactoryService _viewModelFactoryService;

    internal EntityContainingViewModelNavigation(INavigationFactory<TFrom, TTo> navigationFactory,
        INavigationService navigationService, ViewModelsFactoryService viewModelFactoryService)
    {
        _navigationFactory = navigationFactory;
        _navigationService = navigationService;
        _viewModelFactoryService = viewModelFactoryService;
    }

    public async Task NavigateAsync<TAppViewModel>(TFrom entity) where TAppViewModel : ApplicationBaseViewModel, IEntityContainingViewModel<TTo>
    {
        var toEntity = await _navigationFactory.CreateViewModelAsync(entity);
        var entityContainingViewModel = _viewModelFactoryService.CreateViewModel(typeof(TAppViewModel));
        ((IEntityContainingViewModel<TTo>)entityContainingViewModel!).Entity = toEntity;
        _navigationService.NavigateTo(entityContainingViewModel);
    }
}