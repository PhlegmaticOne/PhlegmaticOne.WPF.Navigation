using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

public interface INavigationService
{
    ApplicationBaseViewModel CurrentViewModel { get; }
    event EventHandler<ApplicationBaseViewModel> ViewModelChanged;
    void NavigateTo(Type applicationViewModelType);
    void NavigateTo<T>() where T : ApplicationBaseViewModel;
    void NavigateTo(ApplicationBaseViewModel applicationBaseViewModel);
}