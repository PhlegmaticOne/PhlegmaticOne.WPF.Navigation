using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.Extensions;

internal class NavigationConfigurationFactory
{
    private readonly Func<ApplicationBaseViewModel> _factory;
    internal NavigationConfigurationFactory(Func<ApplicationBaseViewModel> factory, Type viewModelType)
    {
        ViewModelType = viewModelType;
        _factory = factory;
    }
    internal Type ViewModelType { get; }
    public ApplicationBaseViewModel Create() => _factory.Invoke();
}