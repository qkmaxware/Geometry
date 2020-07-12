using System;
using System.Linq;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

public class Capsule : Mesh {

    private static Vec3 ToCartesian(double zrot, double inc, double r) {
        double sTheta = Math.Sin(inc);
        return new Vec3(
            r * sTheta * Math.Cos(zrot),
            r * sTheta * Math.Sin(zrot),
            r * Math.Cos(inc)
        );
    }

    private static List<Triangle> GenerateHemisphere(
        double radius, 
        Vec3 centre, 
        int horiResolution, 
        int vertResolution
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

    private static List<Triangle> GenerateCylinder(double upperRadius, double lowerRadius, double h, Vec3 centre, int resolution) {
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
                te -- ti
                |   /  |
                |  /   |
                be -- bi
            */
            // Create top points
            Vec3 te = new Vec3(xe * upperRadius, ye * upperRadius, hStep) + centre;
            Vec3 ti = new Vec3(xi * upperRadius, yi * upperRadius, hStep) + centre;
            // Create bottom points
            Vec3 be = new Vec3(xe * lowerRadius, ye * lowerRadius, -hStep) + centre;
            Vec3 bi = new Vec3(xi * lowerRadius, yi * lowerRadius, -hStep) + centre;

            // Create triangles
            triangles.Add(new Triangle(be, te, ti));
            triangles.Add(new Triangle(be, ti, bi));
        }

        return triangles;
    }

    /// <summary>
    /// Create a capsule
    /// </summary>
    /// <param name="radius">capsule hemisphere radius</param>
    /// <param name="height">capsule total height</param>
    /// <param name="centre">centre of the capsule</param>
    /// <param name="horizontalResolution">longitude subdivision levels</param>
    /// <param name="verticalResolution">latitude subdivision level</param>
    public Capsule(double radius, double height, Vec3 centre, int horizontalResolution = 8, int verticalResolution = 8) {
        height = Math.Max(height, 2*radius); // height must be greater than 2*radius or else its a squashed sphere
        
        // Capsule is a cylinder
        var cylinderHeight = height - 2 * radius;
        var cylinder = GenerateCylinder(radius, radius, cylinderHeight, centre, horizontalResolution);
        this.AppendRange(cylinder);

        // Plus 2 spheres
        Vec3 delta = cylinderHeight * 0.5 * Vec3.K;
        var topSphere = GenerateHemisphere(radius, centre + delta, horizontalResolution, verticalResolution);
        this.AppendRange(topSphere);
        this.AppendRange(topSphere.Select(tri => new Triangle(tri.Item1.Flipped, tri.Item3.Flipped ,tri.Item2.Flipped)));
    }

}

}