using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Tube mesh
/// </summary>
public class Tube : ListMesh {
    private static List<Triangle> Generate(double outerRadius, double innerRadius, double h, Vec3 centre, int resolution) {
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
            // Outer tube
            // Create top points
            Vec3 te = new Vec3(xe * outerRadius, ye * outerRadius, hStep) + centre;
            Vec3 ti = new Vec3(xi * outerRadius, yi * outerRadius, hStep) + centre;
            // Create bottom points
            Vec3 be = new Vec3(xe * outerRadius, ye * outerRadius, -hStep) + centre;
            Vec3 bi = new Vec3(xi * outerRadius, yi * outerRadius, -hStep) + centre;

            // Create triangles
            triangles.Add(new Triangle(be, te, ti));
            triangles.Add(new Triangle(be, ti, bi));

            // Inner tube
            // Create top points
            Vec3 ite = new Vec3(xe * innerRadius, ye * innerRadius, hStep) + centre;
            Vec3 iti = new Vec3(xi * innerRadius, yi * innerRadius, hStep) + centre;
            // Create bottom points
            Vec3 ibe = new Vec3(xe * innerRadius, ye * innerRadius, -hStep) + centre;
            Vec3 ibi = new Vec3(xi * innerRadius, yi * innerRadius, -hStep) + centre;

            // Create triangles
            triangles.Add(new Triangle(ibe, iti, ite));
            triangles.Add(new Triangle(ibe, ibi, iti));

            // Joiners
            triangles.Add(new Triangle(te, ite, iti));
            triangles.Add(new Triangle(te, iti, ti));

            triangles.Add(new Triangle(be, ibi, ibe));
            triangles.Add(new Triangle(be, bi, ibi));
        }

        return triangles;
    }

    /// <summary>
    /// Cylinder with different radii for top and bottom caps
    /// </summary>
    /// <param name="outerRadius">outer radius</param>
    /// <param name="innerRadius">inner radius</param>
    /// <param name="height">height</param>
    /// <param name="centre">centre</param>
    /// <param name="resolution">subdivision level</param>
    public Tube (double outerRadius, double innerRadius, double height, Vec3 centre, int resolution = 8) : base(Generate(outerRadius, innerRadius, height, centre, resolution)) {}


}

}