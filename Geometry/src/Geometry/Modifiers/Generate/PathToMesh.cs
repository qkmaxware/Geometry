using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// PathToMesh modifier creates a tube that follows a path
/// </summary>
public class PathToMesh : PolygonGeneratorModifier<IInterpolatedPath3> {
    /// <summary>
    /// Radius of the path tube
    /// </summary>
    public float TubeRadius {get; set;}

    /// <summary>
    /// Distance to move along the path each step
    /// </summary>
    public float StepDistance {get; set;}

    /// <summary>
    /// Tube quality
    /// </summary>
    public int Resolution {get; set;}

    /// <summary>
    /// Create a new PathToMesh modifier
    /// </summary>
    /// <param name="path">path to transform</param>
    /// <param name="radius">radius of the path's tube</param>
    /// <param name="resolution">smoothness quality of the tube</param>
    /// <param name="step">number of steps in the path's segment</param>
    /// <returns>new modifier</returns>
    public PathToMesh (IInterpolatedPath3 path, float radius, float step, int resolution = 8) : base(path) {
        this.TubeRadius = radius;
        this.StepDistance = step;
        this.Resolution = resolution;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        double angularStep = 2 * Math.PI / Resolution;
        var basis = new Basis();
        var id = Transformation.Identity();
        basis.Transform = Quat.FromToRotation(Vec3.K, this.Original.Tangent(0)) * id;

        for (float t = StepDistance; t <= 1; t+=StepDistance) {
            var previousFrontVec = basis.Y;
            var previousSideVec = basis.X;

            basis.Transform = Quat.FromToRotation(Vec3.K, this.Original.Tangent(t)) * id;
            var nextFrontVec = basis.Y;
            var nextSideVec = basis.X;
            
            for (int i = 1; i <= Resolution; i++) {
                // Position on 2D XY plane
                double previousAngle = (i - 1) * angularStep;
                double xi = TubeRadius * Math.Cos(previousAngle);
                double yi = TubeRadius * Math.Sin(previousAngle);

                double nextAngle = (i) * angularStep;
                double xe = TubeRadius * Math.Cos(nextAngle);
                double ye = TubeRadius * Math.Sin(nextAngle);

                // Convert to 3D positions
                Vec3 previousMidline = this.Original[t - StepDistance];
                Vec3 nextMidline = this.Original[t];

                Vec3 be = previousMidline + previousFrontVec * ye + previousSideVec * xe;
                Vec3 bi = previousMidline + previousFrontVec * yi + previousSideVec * xi;
                Vec3 te = nextMidline + nextFrontVec * ye + nextSideVec * xe;
                Vec3 ti = nextMidline + nextFrontVec * yi + nextSideVec * xi;

                /* CREATE TRIANGLES
                    te -- ti
                    |   /  |
                    |  /   |
                    be -- bi
                */
                yield return new Triangle(be, te, ti);
                yield return new Triangle(be, ti, bi);
            }
        }
    }
}

}