using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Cone shaped mesh
/// </summary>
public class Cone : ParameterizedMesh {
    protected override IMesh Generate() {
        List<Triangle> triangles = new List<Triangle>();
        double step = 2 * Math.PI / resolution;
        double hStep = h;

        for (int i = 1; i <= resolution; i++) {
            double prevAngle = (i - 1) * step;
            double xi = Math.Cos(prevAngle);
            double yi = Math.Sin(prevAngle);

            double angle = i * step;
            double xe = Math.Cos(angle);
            double ye = Math.Sin(angle);

            /*
                   top 
                 /    \
                be -- bi
                 \    /
                 bottom
            */
            // Create bottom points
            Vec3 be = new Vec3(xe * lowerRadius, ye * lowerRadius, 0) + centre;
            Vec3 bi = new Vec3(xi * lowerRadius, yi * lowerRadius, 0) + centre;
            // Create endpoints
            Vec3 top = new Vec3(0, 0, hStep) + centre;
            Vec3 bottom = new Vec3(0, 0, 0) + centre;

            // Create triangles
            triangles.Add(new Triangle(be, bi, bottom));
            triangles.Add(new Triangle(be, top, bi));
        }

        return new ListMesh(triangles);
    }

    double lowerRadius;
    public double Radius {
        get => lowerRadius;
        set { lowerRadius = value; Rebuild(); }
    }
    double h;
    public double Height {
        get => h;
        set { h = value; Rebuild(); }
    }
    Vec3 centre;
    public Vec3 Centre {
        get => centre;
        set { centre = value; Rebuild(); }
    }
    int resolution;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }

    /// <summary>
    /// Create a new cone
    /// </summary>
    /// <param name="radius">radius</param>
    /// <param name="height">height</param>
    /// <param name="centre">centre of the cone</param>
    /// <param name="resolution">subdivision level</param>
    public Cone (double radius, double height, Vec3 centre, int resolution = 8) {
        this.lowerRadius = radius;
        this.h = height;
        this.centre = centre;
        this.resolution = resolution;
        Rebuild();
    }
    
}

}
