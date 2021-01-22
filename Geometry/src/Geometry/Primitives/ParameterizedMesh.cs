using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Class whose mesh data is rebuilt when its defining parameters change
/// </summary>
public abstract class ParameterizedMesh : IMesh {

    private IMesh? cachedMesh = null;

    /// <summary>
    /// Trigger a rebuild of the mesh
    /// </summary>
    public void Rebuild() {
        cachedMesh = null;
    }

    protected abstract IMesh Generate();

    public IEnumerator<Triangle> GetEnumerator() {
        // If dirty, regenerate mesh
        if (cachedMesh == null) {
            cachedMesh = Generate();
        }

        // If null, return no triangles
        if (cachedMesh == null) {
            return Enumerable.Empty<Triangle>().GetEnumerator();
        } 
        // Return triangles
        else {
            return cachedMesh.GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

}