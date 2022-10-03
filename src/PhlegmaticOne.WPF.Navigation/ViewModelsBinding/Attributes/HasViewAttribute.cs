namespace PhlegmaticOne.WPF.Navigation.ViewModelsBinding.Attributes;

/// <summary>
/// Attribute for marking ViewModels that must be binded Views
/// </summary>
public class HasViewAttribute : Attribute
{
    public HasViewAttribute() { }
    public HasViewAttribute(string? viewTypeNameStart) => ViewTypeNameStart = viewTypeNameStart;
    /// <summary>
    /// View type name start
    /// </summary>
    public string? ViewTypeNameStart { get; }
}
