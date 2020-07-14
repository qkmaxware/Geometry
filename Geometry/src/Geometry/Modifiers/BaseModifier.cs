using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Base class for mesh modification decorators
/// </summary>
public abstract class BaseModifier : IEnumerable<Triangle> {

    /// <summary>
    /// The mesh to be decorated
    /// </summary>
    public IEnumerable<Triangle> OriginalMesh {get; private set;}

    /// <summary>
    /// Add a new modifier to the mesh
    /// </summary>
    /// <param name="originalMesh">original mesh</param>
    public BaseModifier(IEnumerable<Triangle> originalMesh) {
        this.OriginalMesh = originalMesh;
    }

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