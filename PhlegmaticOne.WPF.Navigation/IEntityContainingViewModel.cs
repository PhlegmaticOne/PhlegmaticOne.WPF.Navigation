using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

public interface IEntityContainingViewModel<T> where T : EntityBaseViewModel
{
    T Entity { get; set; }
}