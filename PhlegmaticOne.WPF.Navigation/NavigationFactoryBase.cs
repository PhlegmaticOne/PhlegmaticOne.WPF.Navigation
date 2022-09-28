using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.Exceptions;
using PhlegmaticOne.WPF.Navigation.ViewModelFactories;

namespace PhlegmaticOne.WPF.Navigation;

public abstract class NavigationFactoryBase<TFrom, TTo> : INavigationFactory<TFrom, TTo>
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
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