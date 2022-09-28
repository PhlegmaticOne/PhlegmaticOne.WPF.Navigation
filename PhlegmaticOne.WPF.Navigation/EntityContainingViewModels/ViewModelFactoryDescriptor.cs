namespace PhlegmaticOne.WPF.Navigation.EntityContainingViewModels;

internal class ViewModelFactoryDescriptor : IEquatable<ViewModelFactoryDescriptor>
{
    private readonly Type _entityViewModelType;
    private readonly Type _applicationViewModelType;

    internal ViewModelFactoryDescriptor(Type entityViewModelType, Type applicationViewModelType)
    {
        _entityViewModelType = entityViewModelType;
        _applicationViewModelType = applicationViewModelType;
    }

    public bool Equals(ViewModelFactoryDescriptor? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _entityViewModelType == other._entityViewModelType && _applicationViewModelType == other._applicationViewModelType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ViewModelFactoryDescriptor)obj);
    }

    public override int GetHashCode() => HashCode.Combine(_entityViewModelType, _applicationViewModelType);
}