using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {


public class PathDeform : BaseModifier {

    public Vec3 DeformationAxis {get; private set;}    
    public IInterpolatedPath3 Path {get; private set;}

    public PathDeform(Vec3 deformationAxis, IInterpolatedPath3 path, IEnumerable<Triangle> mesh) : base(mesh) {
        this.Path = path;
        this.DeformationAxis = deformationAxis;
    }

    private Vec3 WarpOnAxis (Vec3 position) {
        var distanceFactor = position.ScalarProjectionOnto(DeformationAxis);
        var pointOnAxis = Path[distanceFactor];
        return (position - DeformationAxis * distanceFactor) + pointOnAxis; 
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        foreach (var tri in this.OriginalMesh) {
            var v1 = WarpOnAxis(tri.Item1);
            var v2 = WarpOnAxis(tri.Item2);
            var v3 = WarpOnAxis(tri.Item3);

            yield return new Triangle(v1, v2, v3);
        }
    }
}

}