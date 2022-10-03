using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Extensions.Factories;

namespace PhlegmaticOne.WPF.Navigation.ViewModelFactories;

internal class ViewModelsFactoryService
{
    private readonly Dictionary<Type, NavigationConfigurationFactory> _applicationBaseViewModels;
    internal ViewModelsFactoryService(Dictionary<Type, NavigationConfigurationFactory> applicationBaseViewModels) =>
        _applicationBaseViewModels = applicationBaseViewModels;

    public ApplicationBaseViewModel? CreateViewModel(Type viewModelType) =>
        _applicationBaseViewModels.TryGetValue(viewModelType, out var viewModel) ? viewModel.Create() : null;
}