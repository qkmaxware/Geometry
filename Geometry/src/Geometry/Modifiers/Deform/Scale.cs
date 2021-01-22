using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Scale modifier to scale a geometry
/// </summary>
public class Scale : PolygonDeformationModifier {
    /// <summary>
    /// Scale multiplier
    /// </summary>
    public Vec3 ScaleFactor {get; set;}

    /// <summary>
    /// Create an scale modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="scale">scalar factors</param>
    public Scale (IMesh mesh, Vec3 scale) : base (mesh) {
        this.ScaleFactor = scale;
    }
    /// <summary>
    /// Create an scale modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="scale">uniform scale factor</param>
    public Scale (IMesh mesh, float scale) : base (mesh) {
        this.ScaleFactor = Vec3.One * scale;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        var matrix = Transformation.Scale(ScaleFactor);
        foreach  (var tri in this.Original) {
            yield return tri.Transform(matrix);
        }
    }

}

}