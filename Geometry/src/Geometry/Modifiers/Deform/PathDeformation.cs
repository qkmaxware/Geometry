using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {


public class PathDeform : PolygonDeformationModifier {

    public Vec3 DeformationAxis {get; set;}    
    public IInterpolatedPath3 Path {get; set;}

    public PathDeform(Vec3 deformationAxis, IInterpolatedPath3 path, IMesh mesh) : base(mesh) {
        this.Path = path;
        this.DeformationAxis = deformationAxis;
    }

    private Vec3 WarpOnAxis (Vec3 position) {
        var distanceFactor = position.ScalarProjectionOnto(DeformationAxis);
        var t = distanceFactor / Vec3.Distance(Path.Start, this.Path.End);
        var pointOnAxis = Path[t];
        return (position - DeformationAxis * distanceFactor) + pointOnAxis; 
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        foreach (var tri in this.Original) {
            var v1 = WarpOnAxis(tri.Item1);
            var v2 = WarpOnAxis(tri.Item2);
            var v3 = WarpOnAxis(tri.Item3);

            yield return new Triangle(v1, v2, v3);
        }
    }
}

}