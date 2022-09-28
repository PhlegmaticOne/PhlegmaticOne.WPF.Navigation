using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Navigation.ChainNavigation;
using PhlegmaticOne.WPF.Navigation.EntityContainingViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.Navigation;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation.Extensions;

public static class NavigationExtensions
{
    private static NavigationConfiguration? _navigationExtensionsHelpers;
    private static EntityContainingNavigationConfiguration? _entityContainingNavigationHelpers;
    private static bool _isConfigured;
    public static NavigationConfiguration AddNavigation(this IServiceCollection serviceCollection)
    {
        if (_isConfigured)
        {
            throw new NavigationException("You have already add navigation. It also may be ChainedNavigation");
        }

        _navigationExtensionsHelpers ??= new(serviceCollection);

        serviceCollection.AddSingleton<INavigationService, NavigationService>(x =>
        {
            var navigationViewModelFactories = x.GetServices(typeof(NavigationConfigurationFactory))
                .Select(service => (NavigationConfigurationFactory) service!)
                .ToDictionary(factory => factory.ViewModelType);
            var viewModelFactoryService = new ViewModelsFactoryService(navigationViewModelFactories);
            return new NavigationService(viewModelFactoryService);
        });

        _isConfigured = true;
        return _navigationExtensionsHelpers;
    }
    public static NavigationConfiguration AddChainNavigation(this IServiceCollection serviceCollection)
    {
        if (_isConfigured)
        {
            throw new NavigationException("You have already add navigation");
        }

        _navigationExtensionsHelpers ??= new(serviceCollection);

        serviceCollection.AddSingleton<IChainNavigationService, ChainNavigationService>(x =>
        {
            var navigationViewModelFactories = x.GetServices(typeof(NavigationConfigurationFactory))
                .Select(service => (NavigationConfigurationFactory)service!)
                .ToDictionary(factory => factory.ViewModelType);
            var viewModelFactoryService = new ViewModelsFactoryService(navigationViewModelFactories);
            return new ChainNavigationService(viewModelFactoryService);
        });

        serviceCollection.AddSingleton<INavigationService, IChainNavigationService>(x => x.GetRequiredService<IChainNavigationService>());

        _isConfigured = true;

        return _navigationExtensionsHelpers;
    }

    public static EntityContainingNavigationConfiguration AddEntityContainingNavigation(this IServiceCollection serviceCollection)
    {
        _entityContainingNavigationHelpers ??= new(serviceCollection);

        if (serviceCollection.Any(x => x.ServiceType == typeof(INavigationService)) == false)
        {
            throw new NavigationException("Before adding entity containing navigation you have to add navigation or chained navigation");
        }

        serviceCollection.AddSingleton(x =>
        {
            var navigationFactories = _entityContainingNavigationHelpers.NavigationFactoriesTypes
                .Select(navigationFactoryType => (INavigationFactory)x.GetRequiredService(navigationFactoryType))
                .ToDictionary(n =>
                    {
                        var genericTypes = n.GetType().BaseType!.GetGenericArguments();
                        var fromType = genericTypes[0];
                        var toType = genericTypes[1];
                        return new ViewModelFactoryDescriptor(fromType, toType);
                    },
                    n => n);
            return navigationFactories;
        });

        serviceCollection
            .AddTransient<IEntityContainingViewModelsNavigationService, EntityContainingViewModelNavigationService>(x =>
            {
                var result = new Dictionary<Type, NavigationConfigurationFactory>();
                foreach (var service in x.GetServices(typeof(EntityContainingNavigationConfigurationFactory)))
                {
                    var factory = (EntityContainingNavigationConfigurationFactory)service;
                    result.Add(factory.ViewModelType, factory);
                }
                var viewModelFactoryService = new ViewModelsFactoryService(result);

                var navigationFactories =
                    x.GetRequiredService<Dictionary<ViewModelFactoryDescriptor, INavigationFactory>>();

                var navigationService = x.GetRequiredService<INavigationService>();

                return new EntityContainingViewModelNavigationService(navigationService, viewModelFactoryService, navigationFactories);
            });
        return _entityContainingNavigationHelpers;
    }
}