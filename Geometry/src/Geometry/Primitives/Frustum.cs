using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Conic frustum mesh
/// </summary>
public class Frustum : ParameterizedMesh {

    protected override IMesh Generate() {
        List<Triangle> tris = new List<Triangle>();
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

            tris.Add(new Triangle(t1, t2, b2));
            tris.Add(new Triangle(t1, b2, b1));
            tris.Add(new Triangle(b1, bottomCentre, b2));
            tris.Add(new Triangle(t1, topCentre, t2));
        }
        
        return new ListMesh(tris);
    }

    double radius;
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }
    double height;
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }
    double ratio;
    public double Ratio {
        get => ratio;
        set { ratio = value; Rebuild(); }
    }
    Vec3 centre;
    public Vec3 Centre {
        get => centre;
        set { centre = value; Rebuild(); }
    }
    int resolution = 4;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }

    /// <summary>
    /// Create a frustum
    /// </summary>
    /// <param name="size">size of the cube</param>
    /// <param name="centre">centre of the cube</param>
    public Frustum (double radius, double height, double ratio, Vec3 centre, int resolution = 4) {
        this.radius = radius;
        this.height = height;
        this.ratio = ratio;
        this.centre = centre;
        this.resolution = resolution;
        Rebuild();
    }
}

}