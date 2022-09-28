using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

public interface IViewModelFactoryService
{
    ApplicationBaseViewModel? CreateViewModel(Type viewModelType);
}