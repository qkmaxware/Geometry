using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Explode modifier pushes apart triangles
/// </summary>
public class Explode : PolygonDeformationModifier {

    /// <summary>
    /// Explosion distance
    /// </summary>
    /// <value>distance</value>
    public float Distance {get; set;}

    /// <summary>
    /// New explosion modifier
    /// </summary>
    /// <param name="mesh">mesh to explode</param>
    /// <param name="distance">distance to explode</param>
    /// <returns>exploaded mesh modifier</returns>
    public Explode (IMesh mesh, float distance) : base(mesh) {
        this.Distance = distance;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        foreach  (var tri in this.Original) {
            var normal = tri.Normal;
            yield return new Triangle(
                tri.Item1 + normal * Distance,
                tri.Item2 + normal * Distance,
                tri.Item3 + normal * Distance
            );
        }
    }

}

}