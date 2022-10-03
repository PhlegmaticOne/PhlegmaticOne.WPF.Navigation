using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;
using System.Reflection;

namespace PhlegmaticOne.WPF.Navigation.Extensions.Helpers;

internal class EntityContainingNavigationConfigurationHelpers
{
    private readonly IServiceCollection _serviceCollection;
    internal readonly List<Type> NavigationFactoriesTypes;
    internal EntityContainingNavigationConfigurationHelpers(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        NavigationFactoriesTypes = new();
    }

    internal EntityContainingNavigationConfigurationHelpers AddNavigationFactories(Assembly navigationFactoriesContainingAssembly)
    {
        var navigationFactoriesTypes = navigationFactoriesContainingAssembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(INavigationFactory)) &&
                        x.IsInterface == false &&
                        x.IsAbstract == false)
            .ToList();

        AddTypes(navigationFactoriesTypes);

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