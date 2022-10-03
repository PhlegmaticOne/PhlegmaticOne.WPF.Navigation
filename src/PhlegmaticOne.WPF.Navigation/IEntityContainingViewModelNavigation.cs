using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Contract for navigation to IEntityContainingViewModel from specified EntityViewModel
/// </summary>
/// <typeparam name="TFrom">EntityViewModel type to navigate from</typeparam>
/// <typeparam name="TTo">EntityViewModel type that present in navigating IEntityContainingViewModel to navigate to</typeparam>
public interface IEntityContainingViewModelNavigation<in TFrom, TTo>
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
    /// <summary>
    /// Navigates to specified IEntityContainingViewModel from specified EntityViewModel
    /// </summary>
    /// <typeparam name="TAppViewModel">IEntityContainingViewModel type to navigate to</typeparam>
    /// <param name="entity">EntityViewModel to navigate from</param>
    Task NavigateAsync<TAppViewModel>(TFrom entity) where TAppViewModel : ApplicationBaseViewModel, IEntityContainingViewModel<TTo>;
}