using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Cylinder mesh
/// </summary>
public class Cylinder : Mesh {
    private static List<Triangle> Generate(double upperRadius, double lowerRadius, double h, Vec3 centre, int resolution) {
        List<Triangle> triangles = new List<Triangle>();
        double step = 2 * Math.PI / resolution;
        double hStep = h / 2;

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
                te -- ti
                |   /  |
                |  /   |
                be -- bi
                 \    /
                 bottom
            */
            // Create top points
            Vec3 te = new Vec3(xe * upperRadius, ye * upperRadius, hStep) + centre;
            Vec3 ti = new Vec3(xi * upperRadius, yi * upperRadius, hStep) + centre;
            // Create bottom points
            Vec3 be = new Vec3(xe * lowerRadius, ye * lowerRadius, -hStep) + centre;
            Vec3 bi = new Vec3(xi * lowerRadius, yi * lowerRadius, -hStep) + centre;
            // Create endpoints
            Vec3 top = new Vec3(0, 0, hStep) + centre;
            Vec3 bottom = new Vec3(0, 0, -hStep) + centre;

            // Create triangles
            triangles.Add(new Triangle(be, te, ti));
            triangles.Add(new Triangle(be, ti, bi));

            triangles.Add(new Triangle(te, top, ti));
            triangles.Add(new Triangle(be, bi, bottom));
        }

        return triangles;
    }

    /// <summary>
    /// Cylinder with different radii for top and bottom caps
    /// </summary>
    /// <param name="upperRadius">top cap radius</param>
    /// <param name="lowerRadius">bottom cap radius</param>
    /// <param name="height">height</param>
    /// <param name="centre">centre</param>
    /// <param name="resolution">subdivision level</param>
    public Cylinder (double upperRadius, double lowerRadius, double height, Vec3 centre, int resolution = 8) : base(Generate(upperRadius, lowerRadius, height, centre, resolution)) {}

    /// <summary>
    /// Cylinder with uniform radius for top and bottom caps
    /// </summary>
    /// <param name="radius">radius</param>
    /// <param name="height">height</param>
    /// <param name="centre">centre</param>
    /// <param name="resolution">subdivision level</param>
    public Cylinder (double radius, double height, Vec3 centre, int resolution = 8) : base(Generate(radius, radius, height, centre, resolution)) {}

}

}