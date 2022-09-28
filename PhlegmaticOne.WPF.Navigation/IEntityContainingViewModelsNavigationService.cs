using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

public interface IEntityContainingViewModelsNavigationService
{
    IEntityContainingViewModelNavigation<TFrom, TTo> From<TFrom, TTo>()
        where TFrom : EntityBaseViewModel
        where TTo : EntityBaseViewModel;
}