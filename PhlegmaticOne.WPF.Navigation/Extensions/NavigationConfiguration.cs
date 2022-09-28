using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Extensions;

public class NavigationConfiguration
{
    private readonly IServiceCollection _serviceCollection;
    public NavigationConfiguration(IServiceCollection serviceCollection) => _serviceCollection = serviceCollection;
    public IServiceCollection UsingApplicationViewModelsFrom(Assembly viewModelsContainingAssembly)
    {
        var viewModelTypes = viewModelsContainingAssembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(ApplicationBaseViewModel)) &&
                        x.GetInterfaces().All(i => !i.IsGenericType || i.GetGenericTypeDefinition() != typeof(IEntityContainingViewModel<>)) && 
                        x.IsAbstract == false)
            .ToList();

        foreach (var viewModelType in viewModelTypes)
        {
            _serviceCollection.AddSingleton(viewModelType);
            _serviceCollection.AddSingleton(x =>
                new NavigationConfigurationFactory(() => (ApplicationBaseViewModel) x.GetRequiredService(viewModelType), viewModelType));
        }

        return _serviceCollection;
    }
}