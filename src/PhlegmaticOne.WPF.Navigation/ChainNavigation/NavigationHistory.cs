using PhlegmaticOne.WPF.Core.ViewModels;

namespace PhlegmaticOne.WPF.Navigation.ChainNavigation;

internal class NavigationHistory
{
    internal void Move(NavigationMoveDirection navigationMoveDirection) => Current = navigationMoveDirection switch
    {
        NavigationMoveDirection.Back when CanMoveBack => Current!.Previous,
        NavigationMoveDirection.Forward when CanMoveForward => Current!.Next,
        _ => Current
    };

    internal ViewModelNode? Current { get; set; }
    internal bool CanMoveBack => Current?.Previous is not null;
    internal bool CanMoveForward => Current?.Next is not null;
    internal void Add(ApplicationBaseViewModel viewModel)
    {
        if (Current is null)
        {
            Current = new ViewModelNode(viewModel);
            return;
        }

        var node = new ViewModelNode(viewModel);
        Current.Next = node;
        node.Previous = Current;
        Current = node;
    }

    internal void Reset() => Current = null;
}