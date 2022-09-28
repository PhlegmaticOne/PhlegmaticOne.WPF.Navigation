using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

public interface IEntityContainingViewModelNavigation<in TFrom, TTo>
    where TFrom : EntityBaseViewModel
    where TTo : EntityBaseViewModel
{
    Task NavigateAsync<TAppViewModel>(TFrom entity) where TAppViewModel : IEntityContainingViewModel<TTo>;
}