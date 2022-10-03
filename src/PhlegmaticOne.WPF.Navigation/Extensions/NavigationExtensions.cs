using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PhlegmaticOne.WPF.Navigation.Extensions;

public static class NavigationExtensions
{
    /// <summary>
    /// Adds navigation service to ServiceCollection
    /// </summary>
    /// <param name="serviceCollection">Service collection</param>
    /// <param name="viewModelsAssembly">Assembly to search ViewModels to</param>
    /// <param name="viewsAssembly">Assembly to search Views to</param>
    /// <param name="builderAction">Action over NavigationBuilder to build necessary navigation services</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddNavigation(this IServiceCollection serviceCollection,
        Assembly viewModelsAssembly, Assembly viewsAssembly, Action<NavigationBuilder> builderAction)
    {
        var navigationBuilder = new NavigationBuilder(serviceCollection, viewModelsAssembly, viewsAssembly);
        builderAction?.Invoke(navigationBuilder);
        return serviceCollection;
    }
}