using System;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Conic frustum mesh
/// </summary>
public class Frustum : ListMesh {

    /// <summary>
    /// Create a frustum
    /// </summary>
    /// <param name="size">size of the cube</param>
    /// <param name="centre">centre of the cube</param>
    public Frustum (double radius, double height, double ratio, Vec3 centre, int resolution = 4) : base() {
        var topCentre = new Vec3(0, 0, height);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;

        // Bottom conic frustum
        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var t1 = new Vec3(
                radius * ratio * Math.Cos(preAngle),
                radius * ratio * Math.Sin(preAngle),
                height
            );
            var t2 = new Vec3(
                radius * ratio * Math.Cos(angle),
                radius * ratio * Math.Sin(angle),
                height
            );

            var b1 = new Vec3(
                radius * Math.Cos(preAngle),
                radius * Math.Sin(preAngle),
                0
            );
            var b2 = new Vec3(
                radius * Math.Cos(angle),
                radius * Math.Sin(angle),
                0
            );

            this.Append(new Triangle(t1, t2, b2));
            this.Append(new Triangle(t1, b2, b1));
            this.Append(new Triangle(b1, bottomCentre, b2));
            this.Append(new Triangle(t1, topCentre, t2));
        }
    }
}

}