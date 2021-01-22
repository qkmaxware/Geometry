using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Translate modifier to translate a geometry
/// </summary>
public class Translate : PolygonDeformationModifier {
    /// <summary>
    /// Displacement offset
    /// </summary>
    public Vec3 Offset {get; set;}

    /// <summary>
    /// Create an translation modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="offset">translation offset</param>
    public Translate (IMesh mesh, Vec3 offset) : base (mesh) {
        this.Offset = offset;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        var matrix = Transformation.Offset(Offset);
        foreach  (var tri in this.Original) {
            yield return tri.Transform(matrix);
        }
    }

}

}