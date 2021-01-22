namespace Qkmaxware.Geometry {

/// <summary>
/// Generic mapping from one type of value to another
/// </summary>
/// <typeparam name="From">Type to convert from</typeparam>
/// <typeparam name="To">Type to convert to</typeparam>
public interface IMapping<From, To> {
    To Map(From value);
}

}