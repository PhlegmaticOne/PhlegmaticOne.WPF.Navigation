using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation.Extensions;

public class EntityContainingNavigationConfiguration
{
    private readonly IServiceCollection _serviceCollection;
    private bool _isConfiguredFromAssembly;
    private bool _isConfiguredFromTypes;

    internal readonly List<Type> NavigationFactoriesTypes;
    public EntityContainingNavigationConfiguration(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        NavigationFactoriesTypes = new();
    }

    public EntityContainingNavigationConfiguration UsingApplicationViewModelsFrom(Assembly viewModelsContainingAssembly)
    {
        var viewModelTypes = viewModelsContainingAssembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ApplicationBaseViewModel)) &&
                        x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityContainingViewModel<>)) &&
                        x.IsAbstract == false)
            .ToList();

        foreach (var viewModelType in viewModelTypes)
        {
            _serviceCollection.AddTransient(viewModelType);
            _serviceCollection.AddTransient(x =>
                new EntityContainingNavigationConfigurationFactory(() => (ApplicationBaseViewModel)x.GetRequiredService(viewModelType), viewModelType));
        }

        return this;
    }

    public EntityContainingNavigationConfiguration UsingNavigationFactoriesFrom(Assembly navigationFactoriesContainingAssembly)
    {
        if (_isConfiguredFromTypes)
        {
            throw new NavigationException("You have registered navigation factories from their types");
        }

        var navigationFactoriesTypes = navigationFactoriesContainingAssembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(INavigationFactory)) && 
                        x.IsInterface == false &&
                        x.IsAbstract == false)
            .ToList();

        AddTypes(navigationFactoriesTypes);

        _isConfiguredFromAssembly = true;

        return this;
    }

    public EntityContainingNavigationConfiguration UsingNavigationFactories(params Type[] navigationFactoriesTypes)
    {
        if (_isConfiguredFromAssembly)
        {
            throw new NavigationException("You have registered navigation types from assembly");
        }

        AddTypes(navigationFactoriesTypes);

        _isConfiguredFromTypes = true;

        return this;
    }

    public EntityContainingNavigationConfiguration IncludeNavigationFactory<T>() where T : class, INavigationFactory
    {
        NavigationFactoriesTypes.Add(typeof(T));

        _serviceCollection.AddSingleton<T>();

        return this;
    }

    private void AddTypes(IEnumerable<Type> navigationFactoriesTypes)
    {
        foreach (var navigationFactoryType in navigationFactoriesTypes)
        {
            NavigationFactoriesTypes.Add(navigationFactoryType);
            _serviceCollection.AddSingleton(navigationFactoryType);
        }
    }
}