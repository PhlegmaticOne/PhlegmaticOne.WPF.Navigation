using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Contract for default navigation service
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Current ApplicationViewModel
    /// </summary>
    ApplicationBaseViewModel CurrentViewModel { get; }
    /// <summary>
    /// Raises when current ApplicationViewModel changed. Raises with new ApplicationViewModel setted
    /// </summary>
    event EventHandler<ApplicationBaseViewModel> ViewModelChanged;
    /// <summary>
    /// Navigates to ApplicationViewModel of specified ApplicationViewModel type
    /// </summary>
    /// <param name="applicationViewModelType">Specified ApplicationViewModel type</param>
    void NavigateTo(Type applicationViewModelType);
    /// <summary>
    /// Navigates to ApplicationViewModel of specified ApplicationViewModel type
    /// </summary>
    /// <typeparam name="T">Specified ApplicationViewModel type</typeparam>
    void NavigateTo<T>() where T : ApplicationBaseViewModel;
    /// <summary>
    /// Navigates to specified ApplicationViewModel
    /// </summary>
    /// <param name="applicationBaseViewModel">Specified ApplicationViewModel</param>
    void NavigateTo(ApplicationBaseViewModel applicationBaseViewModel);
}