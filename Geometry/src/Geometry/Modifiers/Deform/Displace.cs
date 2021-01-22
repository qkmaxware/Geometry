using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Displace modifier to displace a geometry's vertices
/// </summary>
public class Displace : PolygonDeformationModifier {

    /// <summary>
    /// Displacement mapping
    /// </summary>
    /// <value>displacement map</value>
    public IMapping<Vec3, float> DisplacementMap {get; set;}

    /// <summary>
    /// Create an scale modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="offset">displacement offset</param>
    public Displace (IMesh mesh, IMapping<Vec3, float> map) : base (mesh) {
        this.DisplacementMap = map;
    }

    private Transformation GetTransformation(Vec3 midpoint, Vec3 position) {
        var dir = (position - midpoint).Normalized;
        var norm = dir.SqrLength == 0 ? Vec3.Zero : dir.Normalized;
        var displacement = this.DisplacementMap.Map(position);
        return Transformation.Offset(norm * displacement);
    }   

    public override IEnumerator<Triangle> GetEnumerator() {
        var bounds = new Box3(this.Original);

        foreach  (var tri in this.Original) {
            yield return new Triangle(
                GetTransformation(bounds.Centre, tri.Item1) * tri.Item1,
                GetTransformation(bounds.Centre, tri.Item2) * tri.Item2,
                GetTransformation(bounds.Centre, tri.Item3) * tri.Item3
            );
        }
    }

}

}