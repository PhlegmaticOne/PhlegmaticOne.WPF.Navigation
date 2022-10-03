using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Contract for navigation to IEntityContainingViewModel
/// </summary>
public interface IEntityContainingViewModelsNavigationService
{
    /// <summary>
    /// Begins navigation to ApplicationViewModel that has specified EntityViewModel from specified EntityViewModel
    /// </summary>
    /// <typeparam name="TFrom">EntityViewModel type to navigate from</typeparam>
    /// <typeparam name="TTo">EntityViewModel type to navigate to</typeparam>
    /// <returns>Object that continues navigation to IEntityContainingViewModel</returns>
    IEntityContainingViewModelNavigation<TFrom, TTo> From<TFrom, TTo>()
        where TFrom : EntityBaseViewModel
        where TTo : EntityBaseViewModel;
}