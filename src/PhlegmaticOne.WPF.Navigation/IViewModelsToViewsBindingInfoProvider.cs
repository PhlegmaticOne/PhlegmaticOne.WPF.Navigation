using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;
using System.Reflection;

namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;

/// <summary>
/// Contract for getting binding ViewModels to Views info
/// </summary>
public interface IViewModelsToViewsBindingInfoProvider
{
    /// <summary>
    /// Gets binding ViewModels to Views info
    /// </summary>
    /// <param name="viewModelsAssembly">Assembly with ViewModel types to search them</param>
    /// <param name="viewsAssembly">Assembly with ViewModel types to search them</param>
    /// <returns>Binding ViewModels to Views info</returns>
    IEnumerable<ViewModelToViewBindingInfo> GetViewModelToViewBindingInfo(Assembly viewModelsAssembly, Assembly viewsAssembly);
}
