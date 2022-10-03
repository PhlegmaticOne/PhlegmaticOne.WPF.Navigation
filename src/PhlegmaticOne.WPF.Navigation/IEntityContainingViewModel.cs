using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation;

/// <summary>
/// Specifies ApplicationViewModel that has one entity of specified type 
/// </summary>
/// <typeparam name="T">Type of entity in ViewModel</typeparam>
public interface IEntityContainingViewModel<T> where T : EntityBaseViewModel
{
    /// <summary>
    /// Entity in ApplicationViewModel
    /// </summary>
    T Entity { get; set; }
}