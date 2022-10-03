namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Models;

/// <summary>
/// Type specified binding info between ViewModel and View
/// </summary>
public class ViewModelToViewBindingInfo
{
    /// <summary>
    /// ViewModel type to bind
    /// </summary>
    public Type ViewModelType { get; }
    /// <summary>
    /// View type to bind
    /// </summary>
    public Type ViewType { get; }
    public ViewModelToViewBindingInfo(Type viewModelType, Type viewType)
    {
        ViewModelType = viewModelType;
        ViewType = viewType;
    }
}
