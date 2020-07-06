using System;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Conic frustum mesh
/// </summary>
public class Frustum : Mesh {

    /// <summary>
    /// Create a cube
    /// </summary>
    /// <param name="size">size of the cube</param>
    /// <param name="centre">centre of the cube</param>
    public Frustum (double radius, double height, double ratio, Vec3 centre, int resolution = 4) : base() {
        var topCentre = new Vec3(0, height, 0);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;

        // Bottom conic frustum
        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var t1 = new Vec3(
                radius * ratio * Math.Cos(preAngle),
                height,
                radius * ratio * Math.Sin(preAngle)
            );
            var t2 = new Vec3(
                radius * ratio * Math.Cos(angle),
                height,
                radius * ratio * Math.Sin(angle)
            );

            var b1 = new Vec3(
                radius * Math.Cos(preAngle),
                0,
                radius * Math.Sin(preAngle)
            );
            var b2 = new Vec3(
                radius * Math.Cos(angle),
                0,
                radius * Math.Sin(angle)
            );

            this.Append(new Triangle(t1, t2, b2));
            this.Append(new Triangle(t1, b2, b1));
            this.Append(new Triangle(b1, bottomCentre, b2));
            this.Append(new Triangle(t1, topCentre, t2));
        }
    }
}

}