using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Convert a NURBS surface or volume to a polygonal mesh
/// </summary>
public class NurbsToMesh : PolygonGeneratorModifier<NurbsSurface> {

    /// <summary>
    /// Number of sampling points
    /// </summary>
    public int GridPoints {get; set;} = 50;

    /// <summary>
    /// Create a modifier to convert the given surface to a mesh
    /// </summary>
    /// <param name="surface">NURBS surface to convert</param>
    /// <returns>conversion modifier</returns>
    public NurbsToMesh(NurbsSurface surface) : base(surface) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        var step = 1.0 / GridPoints;

        for (double i = 0; i < 1; i += step) {
            for (double j = 0; j < 1; j += step) {
                var p1 = this.Original[i, j];
                var p2 = this.Original[i + step, j];
                var p3 = this.Original[i, j + step];
                var p4 = this.Original[i + step, j + step];

                yield return new Triangle(p1, p2, p3);
                yield return new Triangle(p2, p4, p3);
            }
        }
    }

}

}