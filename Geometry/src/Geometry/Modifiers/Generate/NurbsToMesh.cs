using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Convert a NURBS surface or volume to a polygonal mesh
/// </summary>
public class NurbsToMesh : PolygonGeneratorModifier<NurbsSurface> {

    /// <summary>
    /// Create a modifier to convert the given surface to a mesh
    /// </summary>
    /// <param name="surface">NURBS surface to convert</param>
    /// <returns>conversion modifier</returns>
    public NurbsToMesh(NurbsSurface surface) : base(surface) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        throw new NotImplementedException();
    }

}

}