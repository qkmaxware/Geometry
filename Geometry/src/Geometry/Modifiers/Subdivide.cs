using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Subdivide modifier use loop subdivision to increase geometry resolution
/// </summary>
public class Subdivide : BaseModifier {

    /// <summary>
    /// Apply one level of sub-division
    /// </summary>
    /// <param name="original">original geometry</param>
    public Subdivide(IEnumerable<Triangle> original): base (original) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        /*
            v1 --- n1 --- v2
             \    / \     /
              \  /   \   /
              n2  ---  n3
                \     /
                 \   /
                  v3
        */
        foreach (var tri in this.OriginalMesh) {
            // Edges
            var v1 = tri.Item1;
            var v2 = tri.Item2;
            var v3 = tri.Item3;

            // Subdivisions
            var n1 = v1 + 0.5 * tri.Edge12;
            var n2 = v1 + 0.5 * tri.Edge13;
            var n3 = v2 + 0.5 * tri.Edge23;

            yield return new Triangle(v1, n1, n2);
            yield return new Triangle(n1, v2, n3);
            yield return new Triangle(n1, n3, n2);
            yield return new Triangle(n2, n3, v3);
        }
    }

}

}