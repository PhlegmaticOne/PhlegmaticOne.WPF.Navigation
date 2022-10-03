using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.ViewModelFactories;

public interface INavigationFactory
{
    Task<object> CreateViewModelAsync(object entityViewModel);
}

public interface INavigationFactory<in TFrom, TTo> : INavigationFactory
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
    Task<TTo> CreateViewModelAsync(TFrom entityViewModel);
}