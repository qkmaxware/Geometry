using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Mirror modifier to mirror an object over an axis
/// </summary>
public class Mirror : BaseModifier {

    /// <summary>
    /// Axis to mirror over
    /// </summary>
    public Vec3 MirrorAxis {get; private set;}

    /// <summary>
    /// Create a mirror modifier
    /// </summary>
    /// <param name="axis">axis to mirror over</param>
    /// <param name="mesh">mesh to mirror</param>
    public Mirror (Vec3 axis, IEnumerable<Triangle> mesh) : base(mesh) {
        this.MirrorAxis = axis;
    }

    private Vec3 Reflection(Vec3 v) {
        Vec3 a = MirrorAxis;
        return v - 2 * ((Vec3.Dot(v, a)) / (Vec3.Dot(a, a))) * a;
    }
    
    public override IEnumerator<Triangle> GetEnumerator() {
        // Print original
        foreach (var tri in this.OriginalMesh) {
            yield return tri;
        }
        // Print mirrored
        foreach (var tri in this.OriginalMesh) {
            var v1 = Reflection(tri.Item1);
            var v2 = Reflection(tri.Item2);
            var v3 = Reflection(tri.Item3);

            yield return new Triangle(v1, v2, v3);
        }
    }

}

}