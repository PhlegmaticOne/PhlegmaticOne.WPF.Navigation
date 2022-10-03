using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Extensions.Factories;

internal class EntityContainingNavigationConfigurationFactory : NavigationConfigurationFactory
{
    internal EntityContainingNavigationConfigurationFactory(Func<ApplicationBaseViewModel> factory, Type viewModelType) : base(factory, viewModelType) { }
}