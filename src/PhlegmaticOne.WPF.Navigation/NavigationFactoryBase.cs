using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Contract for factories between two EntityViewModels. Used for entity containing ViewModels navigation
/// </summary>
/// <typeparam name="TFrom">EntityViewModel type to navigate from</typeparam>
/// <typeparam name="TTo">EntityViewModel type to navigate to</typeparam>
public abstract class NavigationFactoryBase<TFrom, TTo> : INavigationFactory<TFrom, TTo>
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
    /// <summary>
    /// Creates necessary for naifgation specified EntityViewModel from specified EntityViewModel
    /// </summary>
    /// <param name="entityViewModel"></param>
    /// <returns></returns>
    public abstract Task<TTo> CreateViewModelAsync(TFrom entityViewModel);

    public async Task<object> CreateViewModelAsync(object entityViewModel)
    {
        if (entityViewModel is TFrom from)
        {
            return await CreateViewModelAsync(from);
        }
        throw new NavigationException($"Object of type {entityViewModel.GetType()} cannot be converted to type " +
                                      $"{typeof(TTo)}. You must implement {typeof(NavigationFactoryBase<TFrom, TTo>)} in order " +
                                      $"to create instance of this type");
    }
}