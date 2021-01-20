using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Displace modifier to displace a geometry
/// </summary>
public class Displace : DeformationModifier {
    /// <summary>
    /// Displacement offset
    /// </summary>
    public Vec3 Offset {get; set;}

    /// <summary>
    /// Create an scale modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="offset">displacement offset</param>
    public Displace (IMesh mesh, Vec3 offset) : base (mesh) {
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