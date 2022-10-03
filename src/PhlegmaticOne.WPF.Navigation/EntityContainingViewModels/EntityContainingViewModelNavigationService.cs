using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation.EntityContainingViewModels;

internal class EntityContainingViewModelNavigationService : IEntityContainingViewModelsNavigationService
{
    private readonly Dictionary<ViewModelFactoryDescriptor, INavigationFactory> _navigationFactories;
    private readonly INavigationService _navigationService;
    private readonly ViewModelsFactoryService _viewModelFactoryService;

    internal EntityContainingViewModelNavigationService(INavigationService navigationService,
        ViewModelsFactoryService viewModelFactoryService,
        Dictionary<ViewModelFactoryDescriptor, INavigationFactory> navigationFactories)
    {
        _navigationFactories = navigationFactories;
        _navigationService = navigationService;
        _viewModelFactoryService = viewModelFactoryService;
    }


    public IEntityContainingViewModelNavigation<TFrom, TTo> From<TFrom, TTo>() where TFrom : EntityBaseViewModel where TTo : EntityBaseViewModel
    {
        var viewModelDescriptor = new ViewModelFactoryDescriptor(typeof(TFrom), typeof(TTo));
        if (_navigationFactories.TryGetValue(viewModelDescriptor, out INavigationFactory factory) == false)
        {
            throw new ArgumentException();
        }

        var genericFactory = (INavigationFactory<TFrom, TTo>)factory;
        return new EntityContainingViewModelNavigation<TFrom, TTo>(genericFactory, _navigationService, _viewModelFactoryService);
    }
}