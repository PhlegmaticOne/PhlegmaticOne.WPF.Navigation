using PhlegmaticOne.WPF.Core.ViewModels;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Base;
using PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;
using System.Reflection;
using System.Windows.Controls;

namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding;

/// <summary>
/// Gets ViewModels to Views binding info by explicit binding configuration
/// </summary>
public class HandViewModelsToViewsBindingInfoProvider : IViewModelsToViewsBindingInfoProvider
{
    private readonly List<ViewModelToViewBindingInfo> _viewModelToViewBindingInfos;
    public HandViewModelsToViewsBindingInfoProvider() => _viewModelToViewBindingInfos = new();
    public IEnumerable<ViewModelToViewBindingInfo> GetViewModelToViewBindingInfo(Assembly viewModelsAssembly, Assembly viewsAssembly) =>
        _viewModelToViewBindingInfos;
    /// <summary>
    /// Binds ViewModel to View
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel type to bind to</typeparam>
    /// <typeparam name="TView">View type to bind to</typeparam>
    public HandViewModelsToViewsBindingInfoProvider Bind<TViewModel, TView>()
        where TViewModel : ApplicationBaseViewModel
        where TView : UserControl
    {
        _viewModelToViewBindingInfos.Add(new(typeof(TViewModel), typeof(TView)));
        return this;
    }
}
