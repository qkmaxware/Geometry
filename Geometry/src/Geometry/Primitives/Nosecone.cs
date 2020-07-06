using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Primitive geometry for a nosecone shape based on the common shapes found at https://en.wikipedia.org/wiki/Nose_cone_design
/// </summary>
public class Nosecone : Mesh {
    
     /*
            top
            / \
            te-ti
            | / |
            be-bi
            \ /
            Bottom
    */

    private Nosecone(List<Triangle> tris) : base(tris) {}

    /// <summary>
    /// Conic nosecone
    /// </summary>
    /// <param name="radius">radius of the cone</param>
    /// <param name="height">height of the cone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <returns>conic nosecone</returns>
    public static Nosecone Conic (double radius, double height, int resolution = 8) {
        var topCentre = new Vec3(0, 0, height);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;
        List<Triangle> tris = new List<Triangle>();

        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var x1 = new Vec3(
                radius * Math.Cos(preAngle),
                radius * Math.Sin(preAngle),
                0
            );
            var x2 = new Vec3(
                radius * Math.Cos(angle),
                radius * Math.Sin(angle),
                0
            );

            tris.Add(new Triangle(x2, x1, bottomCentre));
            tris.Add(new Triangle(x2, topCentre, x1));
        }

        return new Nosecone(tris);
    }

    /// <summary>
    /// Biconic Nosecone
    /// </summary>
    /// <param name="coneRadius">radius of the upper cone</param>
    /// <param name="coneLength">length of the upper cone</param>
    /// <param name="frustumRadius">radius of the lower conic frustum</param>
    /// <param name="frustumLength">length of the lower conic frustum</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <returns>biconic nosecone</returns>
    public static Nosecone BiConic(double coneRadius, double coneLength, double frustumRadius, double frustumLength, int resolution = 8) {
        var topCentre = new Vec3(0, 0, coneLength + frustumLength);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;
        List<Triangle> tris = new List<Triangle>();

        // Top cone
        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var x1 = new Vec3(
                coneRadius * Math.Cos(preAngle),
                coneRadius * Math.Sin(preAngle),
                frustumLength
            );
            var x2 = new Vec3(
                coneRadius * Math.Cos(angle),
                coneRadius * Math.Sin(angle),
                frustumLength
            );

            tris.Add(new Triangle(x1, x2, topCentre));
        }

        // Bottom conic frustum
        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var t1 = new Vec3(
                coneRadius * Math.Cos(preAngle),
                coneRadius * Math.Sin(preAngle),
                frustumLength
            );
            var t2 = new Vec3(
                coneRadius * Math.Cos(angle),
                coneRadius * Math.Sin(angle),
                frustumLength
            );

            var b1 = new Vec3(
                frustumRadius * Math.Cos(preAngle),
                frustumRadius * Math.Sin(preAngle),
                0
            );
            var b2 = new Vec3(
                frustumRadius * Math.Cos(angle),
                frustumRadius * Math.Sin(angle),
                0
            );

            tris.Add(new Triangle(t1, b2, t2));
            tris.Add(new Triangle(t1, b1, b2));
            tris.Add(new Triangle(b1, bottomCentre, b2));
        }

        return new Nosecone(tris);
    }

    private static List<Triangle> FuncNosecone(Func<double,double> radiusAtHeightFunc, double radius, double height, int resolution = 8, int segments = 8) {
        var topCentre = new Vec3(0, 0, height);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;
        double yStep = height / segments;
        List<Triangle> tris = new List<Triangle>();

        // Do top cap
        for (var i = 1; i <= resolution; i++) { 
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;
            double heightAtLevel = 1 * yStep;
            double radiusAtLevel = radiusAtHeightFunc(heightAtLevel);

            // Ring
            var x1 = new Vec3(
                radiusAtLevel * Math.Cos(preAngle),
                radiusAtLevel * Math.Sin(preAngle),
                height - heightAtLevel
            );
            var x2 = new Vec3(
                radiusAtLevel * Math.Cos(angle),
                radiusAtLevel * Math.Sin(angle),
                height - heightAtLevel
            );

            tris.Add(new Triangle(x2, topCentre, x1));
        }

        // Do fill 
        for (var y = 1; y < segments; y++) {
            var heightAtLevel =  y * yStep;
            var heightAtNextLevel = (y + 1) * yStep;
            var radiusAtLevel = radiusAtHeightFunc(heightAtLevel);
            var radiusAtNextLevel = radiusAtHeightFunc(heightAtNextLevel);

            for (var i = 1; i <= resolution; i++) { 
                double preAngle = (i - 1) * xStep;
                double angle = i * xStep;

                // Top
                var x1 = new Vec3(
                    radiusAtLevel * Math.Cos(preAngle),
                    radiusAtLevel * Math.Sin(preAngle),
                    height - heightAtLevel
                );
                var x2 = new Vec3(
                    radiusAtLevel * Math.Cos(angle),
                    radiusAtLevel * Math.Sin(angle),
                    height - heightAtLevel
                );
                // Bottom
                var x3 = new Vec3(
                    radiusAtNextLevel * Math.Cos(preAngle),
                    radiusAtNextLevel * Math.Sin(preAngle),
                    height - heightAtNextLevel
                );
                var x4 = new Vec3(
                    radiusAtNextLevel * Math.Cos(angle),
                    radiusAtNextLevel * Math.Sin(angle),
                    height - heightAtNextLevel
                );

                tris.Add(new Triangle(x1, x4, x2));
                tris.Add(new Triangle(x1, x3, x4));
            }
        }

        // Do bottom cap
        for (var i = 1; i <= resolution; i++) {
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;

            var x1 = new Vec3(
                radius * Math.Cos(preAngle),
                radius * Math.Sin(preAngle),
                0
            );
            var x2 = new Vec3(
                radius * Math.Cos(angle),
                radius * Math.Sin(angle),
                0
            );

            tris.Add(new Triangle(x2, x1, bottomCentre));
        }

        return tris;
    }

    /// <summary>
    /// Tangent Ogive Nosecone
    /// </summary>
    /// <param name="radius">nosecone radius</param>
    /// <param name="height">nosecone height</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Tangent Ogive Nosecone</returns>
    public static Nosecone TangentOgive(double radius, double height, int resolution = 8, int segments = 8) {
        var p = (radius * radius + height * height) / (2 * radius);

        return new Nosecone(
            FuncNosecone(
                (h) => {
                    return Math.Sqrt(p*p - (height - h)*(height - h)) + radius - p;
                },
                radius, height, resolution, segments
            )
        );
    }

    /// <summary>
    /// Secant Ogive Nosecone
    /// </summary>
    /// <param name="ogiveRadius">radius of the circle used to define the secant ogive</param>
    /// <param name="conicRadius">base radius</param>
    /// <param name="height">nosecone height</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Secant Ogive Nosecone</returns>
    public static Nosecone SecantOgive(double ogiveRadius, double conicRadius, double height, int resolution = 8, int segments = 8) {
        var pMin = (conicRadius * conicRadius + height * height) / (2 * conicRadius);
        var p = Math.Max(ogiveRadius, pMin);
        var a = Math.Acos(Math.Sqrt(height*height + conicRadius*conicRadius) / (2 * p)) - Math.Atan(conicRadius / height);

        return new Nosecone(
            FuncNosecone(
                (h) => {
                    var pcx = p * Math.Cos(a) - h;
                    return Math.Sqrt(p*p - pcx*pcx) - p * Math.Sin(a);
                },
                conicRadius, height, resolution, segments
            )
        );
    }

    /// <summary>
    /// Elliptical Nosecone
    /// </summary>
    /// <param name="radius">radius of the nosecone</param>
    /// <param name="height">height of the nosecone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Elliptical Nosecone</returns>
    public static Nosecone Elliptical(double radius, double height, int resolution = 8, int segments = 8) {
        return new Nosecone(
            FuncNosecone(
                (h) => {
                    return radius * Math.Sqrt(1 - (h * h / height * height));
                },
                radius, height, resolution, segments
            )
        );
    }

    /// <summary>
    /// Parabolic Nosecone
    /// </summary>
    /// <param name="K">parabolic parametre between 0 and 1</param>
    /// <param name="radius">radius of the nosecone</param>
    /// <param name="height">height of the nosecone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Parabolic Nosecone</returns>
    public static Nosecone Parabolic(double K, double radius, double height, int resolution = 8, int segments = 8) {
        K = K > 1 ? 1 : (K < 0 ? 0 : K); // clamp K

        return new Nosecone(
            FuncNosecone(
                (h) => {
                    var denom = 2 - K;
                    var num = 2 * (h / height) - K * (h / height) * (h / height);
                    return radius * (num / denom);
                },
                radius, height, resolution, segments
            )
        );
    }

    /// <summary>
    /// Power Series Nosecone
    /// </summary>
    /// <param name="K">power series parametre between 0 and 1</param>
    /// <param name="radius">radius of the nosecone</param>
    /// <param name="height">height of the nosecone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Power Series Nosecone</returns>
    public static Nosecone Powerseries (double n, double radius, double height, int resolution = 8, int segments = 8) {
        n = n > 1 ? 1 : (n < 0 ? 0 : n); // clamp n

        return new Nosecone(
            FuncNosecone(
                (h) => {
                    return radius * Math.Pow(h / height, n);
                },
                radius, height, resolution, segments
            )
        );
    }

    /// <summary>
    /// Haack Series Nosecone
    /// </summary>
    /// <param name="K">Haack series parametre between 0 and 1</param>
    /// <param name="radius">radius of the nosecone</param>
    /// <param name="height">height of the nosecone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Haack Series Nosecone</returns>
    public static Nosecone Haack (double C, double radius, double height, int resolution = 8, int segments = 8) {
        C = C > 1 ? 1 : (C < 0 ? 0 : C); // clamp C
        var sqrtPi = Math.Sqrt(Math.PI);

        return new Nosecone(
            FuncNosecone(
                (h) => {
                    var theta = Math.Acos(1 - 2 * h / height);
                    var st = Math.Sin(theta);
                    var s2t = Math.Sign(2 * theta);
                    return (radius / sqrtPi) * Math.Sqrt(theta - s2t / 2 + C * st * st * st);
                },
                radius, height, resolution, segments
            )
        );
    }
}

}