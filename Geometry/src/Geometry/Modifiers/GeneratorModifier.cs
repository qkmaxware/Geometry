using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Represents a modifier that creates new geometry
/// </summary>
/// <typeparam name="T">type of object to create geometry from</typeparam>
public abstract class GeneratorModifier<T> : BaseModifier<T>, IMesh {
                                          //          From -> To
    public GeneratorModifier (T value) : base(value) {}

    /// <summary>
    /// Get triangles of the modified mesh
    /// </summary>
    /// <returns>modifed triangles</returns>
    public abstract IEnumerator<Triangle> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }
}

}