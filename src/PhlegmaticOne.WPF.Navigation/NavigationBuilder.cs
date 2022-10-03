using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.ChainNavigation;
using PhlegmaticOne.WPF.Navigation.EntityContainingViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.Extensions.Factories;
using PhlegmaticOne.WPF.Navigation.Extensions.Helpers;
using PhlegmaticOne.WPF.Navigation.Helpers;
using PhlegmaticOne.WPF.Navigation.Navigation;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Binder;
using System.Reflection;
using System.Windows;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Builds navigation by calling different methods
/// </summary>
public class NavigationBuilder
{
    private bool _isDefaultNavigationConfigured;

    private readonly IServiceCollection _serviceCollection;
    private readonly Assembly _viewModelsAssembly;
    private readonly Assembly _viewsAssembly;
    private readonly EntityContainingNavigationConfigurationHelpers _entityContainingNavigationHelpers;
    internal NavigationBuilder(IServiceCollection serviceCollection,
        Assembly viewModelsAssembly, Assembly viewsAssembly)
    {
        _serviceCollection = serviceCollection;
        _viewModelsAssembly = viewModelsAssembly;
        _viewsAssembly = viewsAssembly;
        _entityContainingNavigationHelpers = new(serviceCollection);
    }
    /// <summary>
    /// Specifies to use default navigation represented by INavigationService interface
    /// </summary>
    /// <param name="viewModelLifetime">Lifetime of ApplicationViewModels wil be created</param>
    /// <exception cref="NavigationException">Throwed if navigation has already configured. It may be because of calling this method twice as well as previous UseChainNavigation method calling</exception>
    public void UseDefaultNavigation(ServiceLifetime viewModelLifetime = ServiceLifetime.Singleton)
    {
        if (_isDefaultNavigationConfigured)
        {
            throw new NavigationException("You have already add navigation. It also may be ChainedNavigation");
        }

        AddDefaultNavigation();

        AddApplicationViewModels(_viewModelsAssembly, viewModelLifetime);

        _isDefaultNavigationConfigured = true;
    }
    /// <summary>
    /// Specifies to use chained navigation represented by IChainNavigationService interface
    /// </summary>
    /// <param name="viewModelLifetime">Lifetime of ApplicationViewModels wil be created</param>
    /// <exception cref="NavigationException">Throwed if navigation has already configured. It may be because of calling this method twice as well as previous UseDefaultNavigation method calling</exception>
    public void UseChainNavigation(ServiceLifetime viewModelLifetime = ServiceLifetime.Singleton)
    {
        if (_isDefaultNavigationConfigured)
        {
            throw new NavigationException("You have already add navigation");
        }

        AddChainNavigation();

        AddApplicationViewModels(_viewModelsAssembly, viewModelLifetime);

        _isDefaultNavigationConfigured = true;
    }

    /// <summary>
    /// Adds entity containing ViewModel navigation represented by IEntityViewModelsNavigationService
    /// </summary>
    /// <param name="navigationFactoriesAssembly">Assembly to search navigation factories</param>
    /// <exception cref="NavigationException">Throwed if default or chained navigation hasn't configured</exception>
    public void AddEntityContainingNavigation(Assembly navigationFactoriesAssembly)
    {
        if (_serviceCollection.Any(x => x.ServiceType == typeof(INavigationService)) == false)
        {
            throw new NavigationException("Before adding entity containing navigation you have to add navigation or chained navigation");
        }

        AddEntityContainingNavigationPrivate();

        AddEntityContainingViewModels(_viewModelsAssembly);

        _entityContainingNavigationHelpers.AddNavigationFactories(navigationFactoriesAssembly);
    }
    /// <summary>
    /// Binds ViewModels to Views
    /// </summary>
    /// <param name="application">WPF Application to bind ViewModels to Views to</param>
    /// <param name="binderInfoProvider">Object that provides ViewModels to Views binding info</param>
    public void BindViewModelsToViews(Application application, IViewModelsToViewsBindingInfoProvider binderInfoProvider)
    {
        ViewModelsToViewsBinder.BindViewModelsToViews(application, _viewModelsAssembly, _viewsAssembly, binderInfoProvider);
    }

    private void AddApplicationViewModels(Assembly viewModelsAssembly, ServiceLifetime viewModelLifetime)
    {
        var viewModelTypes = TypesGetter.GetApplicationViewModels(viewModelsAssembly);

        foreach (var viewModelType in viewModelTypes)
        {
            switch (viewModelLifetime)
            {
                case ServiceLifetime.Singleton or ServiceLifetime.Scoped:
                {
                    _serviceCollection.AddSingleton(viewModelType);
                    _serviceCollection.AddSingleton(x =>
                        new NavigationConfigurationFactory(() =>
                            (ApplicationBaseViewModel)x.GetRequiredService(viewModelType), viewModelType));
                    break;
                }
                case ServiceLifetime.Transient:
                {
                    _serviceCollection.AddTransient(viewModelType);
                    _serviceCollection.AddTransient(x =>
                        new NavigationConfigurationFactory(() =>
                            (ApplicationBaseViewModel)x.GetRequiredService(viewModelType), viewModelType));
                    break;
                }
            }
        }
    }

    private void AddEntityContainingViewModels(Assembly viewModelsAssembly)
    {
        var entityContainingViewModels = TypesGetter.GetEntityContainingViewModels(viewModelsAssembly);

        foreach (var viewModelType in entityContainingViewModels)
        {
            _serviceCollection.AddTransient(viewModelType);
            _serviceCollection.AddTransient(x =>
                new EntityContainingNavigationConfigurationFactory(() => (ApplicationBaseViewModel)x.GetRequiredService(viewModelType), viewModelType));
        }
    }

    private void AddDefaultNavigation()
    {
        _serviceCollection.AddSingleton<INavigationService, NavigationService>(x =>
        {
            var navigationViewModelFactories = x.GetServices(typeof(NavigationConfigurationFactory))
                .Select(service => (NavigationConfigurationFactory)service!)
                .ToDictionary(factory => factory.ViewModelType);
            var viewModelFactoryService = new ViewModelsFactoryService(navigationViewModelFactories);
            return new NavigationService(viewModelFactoryService);
        });
    }

    private void AddChainNavigation()
    {
        _serviceCollection.AddSingleton<IChainNavigationService, ChainNavigationService>(x =>
        {
            var navigationViewModelFactories = x.GetServices(typeof(NavigationConfigurationFactory))
                .Select(service => (NavigationConfigurationFactory)service!)
                .ToDictionary(factory => factory.ViewModelType);
            var viewModelFactoryService = new ViewModelsFactoryService(navigationViewModelFactories);
            return new ChainNavigationService(viewModelFactoryService);
        });

        _serviceCollection.AddSingleton<INavigationService, IChainNavigationService>(x => x.GetRequiredService<IChainNavigationService>());
    }

    private void AddEntityContainingNavigationPrivate()
    {
        _serviceCollection.AddSingleton(x =>
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

        _serviceCollection
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
    }
}
