using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Half sphere mesh
/// </summary>
public class Hemisphere : ListMesh {

    private static Vec3 ToCartesian(double zrot, double inc, double r) {
        double sTheta = Math.Sin(inc);
        return new Vec3(
            r * sTheta * Math.Cos(zrot),
            r * sTheta * Math.Sin(zrot),
            r * Math.Cos(inc)
        );
    }

    private static List<Triangle> Generate(
        double radius, 
        Vec3 centre, 
        int horiResolution, 
        int vertResolution,
        bool useEndCap = true
    ) {
        List<Triangle> triangles = new List<Triangle>();
        
        double xStep = (2 * Math.PI)   / horiResolution;
        double yStep = (0.5 * Math.PI) / (vertResolution - 1);

        /*
                 top
                 / \
                te-ti
                |  /|
               -------
        */

        for (int i = 1; i <= horiResolution; i++) {
            double prevXAngle = (i - 1) * xStep;
            double xAngle = i * xStep;

            for(int j = 1; j < vertResolution; j++) {
                if (j == 1) {
                    // Top Triangle
                    double yAngle = (j) * yStep;

                    Vec3 top = new Vec3(0, 0, radius) + centre;
                    Vec3 be = ToCartesian(xAngle, yAngle, radius) + centre;
                    Vec3 bi = ToCartesian(prevXAngle, yAngle, radius) + centre;

                    triangles.Add(new Triangle(bi, be, top));
                } else {
                    double yAngle = (j) * yStep;
                    double prevYAngle = (j - 1) * yStep;

                    // Bottom Triangle
                    if (useEndCap && j == vertResolution - 1) {
                        Vec3 bottom = new Vec3(0, 0, 0) + centre;
                        Vec3 te = ToCartesian(xAngle, yAngle, radius) + centre;
                        Vec3 ti = ToCartesian(prevXAngle, yAngle, radius) + centre;

                        triangles.Add(new Triangle(ti, bottom, te));
                    }

                    // Middle rectangle
                    {
                        Vec3 te = ToCartesian(xAngle, yAngle, radius) + centre;
                        Vec3 ti = ToCartesian(prevXAngle, yAngle, radius) + centre;

                        Vec3 be = ToCartesian(xAngle, prevYAngle, radius) + centre;
                        Vec3 bi = ToCartesian(prevXAngle, prevYAngle, radius) + centre;

                        triangles.Add(new Triangle(ti, te, be));
                        triangles.Add(new Triangle(bi, ti, be));
                    }
                }
            }
        }

        return triangles;
    }

    /// <summary>
    /// Create a hemisphere
    /// </summary>
    /// <param name="radius">radius</param>
    /// <param name="centre">centre point</param>
    /// <param name="horizontalResolution">longitude subdivision levels</param>
    /// <param name="verticalResolution">latitude subdivision level</param>
    public Hemisphere(double radius, Vec3 centre, int horizontalResolution = 8, int verticalResolution = 8) : base(Generate(radius, centre, horizontalResolution, verticalResolution)) {}
}

}