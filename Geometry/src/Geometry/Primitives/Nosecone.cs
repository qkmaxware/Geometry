using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

public abstract class Nosecone : ParameterizedMesh {

    protected static List<Triangle> FuncNosecone(Func<double,double> radiusAtHeightFunc, double radius, double height, int resolution = 8, int segments = 8) {
        var topCentre = new Vec3(0, 0, height);
        var bottomCentre = Vec3.Zero;
        double xStep = 2 * Math.PI / resolution;
        double yStep = height / segments;
        List<Triangle> tris = new List<Triangle>();

        var firstLevelHeight = 1 * yStep;
        var radiusAtFirstLevelHeight = radiusAtHeightFunc(firstLevelHeight);

        // Do top cap
        for (var i = 1; i <= resolution; i++) { 
            double preAngle = (i - 1) * xStep;
            double angle = i * xStep;
            double heightAtLevel = firstLevelHeight;
            double radiusAtLevel = radiusAtFirstLevelHeight;

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

}

public class ConicNosecone : Nosecone {

    double height;
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }

    double radius;
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }

    int resolution;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }

    /// <summary>
    /// Conic nosecone
    /// </summary>
    /// <param name="radius">radius of the cone</param>
    /// <param name="height">height of the cone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <returns>conic nosecone</returns>
    public ConicNosecone (double radius, double height, int resolution = 8) {
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
    }

    protected override IMesh Generate() {
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

        return new ListMesh(tris);
    }
}

public class BiConicNosecone : Nosecone {
    double coneRadius;
    public double ConeRadius {
        get => coneRadius;
        set { coneRadius = value; Rebuild(); }
    }
    double coneLength;
    public double ConeLength {
        get => coneLength;
        set { coneLength = value; Rebuild(); }
    }
    double frustumRadius;
    public double FrustumRadius {
        get => frustumRadius;
        set { frustumRadius = value; Rebuild(); }
    }
    double frustumLength;
    public double FrustumLength {
        get => frustumLength;
        set { frustumLength = value; Rebuild(); }
    }
    int resolution = 8;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
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
    public BiConicNosecone(double coneRadius, double coneLength, double frustumRadius, double frustumLength, int resolution = 8) {
        this.coneRadius = coneRadius;
        this.coneLength = coneLength;
        this.frustumRadius = frustumRadius;
        this.frustumLength = frustumLength;
        this.resolution = resolution;
    }

    protected override IMesh Generate() {
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

        return new ListMesh(tris);
    }
}

public class TangentOgiveNosecone : Nosecone {

    double radius; 
    public double Radius {
        get => radius;
        set {radius = value; Rebuild(); }
    }
    double height; 
    public double Height {
        get => height;
        set {height = value; Rebuild(); }
    }
    int resolution = 8; 
    public int Resolution {
        get => resolution;
        set {resolution = value; Rebuild(); }
    }
    int segments = 8;
    public int Segments {
        get => segments;
        set {segments = value; Rebuild(); }
    }

    /// <summary>
    /// Tangent Ogive Nosecone
    /// </summary>
    /// <param name="radius">nosecone radius</param>
    /// <param name="height">nosecone height</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Tangent Ogive Nosecone</returns>
    public TangentOgiveNosecone (double radius, double height, int resolution = 8, int segments = 8) {
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate() {
        var p = (radius * radius + height * height) / (2 * radius);

        return new ListMesh(
            FuncNosecone(
                (h) => {
                    return Math.Sqrt(p*p - (height - h)*(height - h)) + radius - p;
                },
                radius, height, resolution, segments
            )
        );
    }
}

public class SecantOgiveNosecone : Nosecone {

    double ogiveRadius; 
    public double OgiveRadius {
        get => ogiveRadius;
        set { ogiveRadius = value; Rebuild(); }
    }
    double conicRadius; 
    public double ConicRadius {
        get => conicRadius;
        set { conicRadius = value; Rebuild(); }
    }
    double height; 
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }
    int resolution = 8; 
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    int segments = 8;
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
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
    public SecantOgiveNosecone(double ogiveRadius, double conicRadius, double height, int resolution = 8, int segments = 8) {
        this.ogiveRadius = ogiveRadius;
        this.conicRadius = conicRadius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate() {
        var pMin = (conicRadius * conicRadius + height * height) / (2 * conicRadius);
        var p = Math.Max(ogiveRadius, pMin);
        var a = Math.Acos(Math.Sqrt(height*height + conicRadius*conicRadius) / (2 * p)) - Math.Atan(conicRadius / height);

        return new ListMesh(
            FuncNosecone(
                (h) => {
                    var pcx = p * Math.Cos(a) - h;
                    return Math.Sqrt(p*p - pcx*pcx) - p * Math.Sin(a);
                },
                conicRadius, height, resolution, segments
            )
        );
    }
}

public class EllipticalNosecone : Nosecone {

    double radius; double height; int resolution = 8; int segments = 8;
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
    }

    /// <summary>
    /// Elliptical Nosecone
    /// </summary>
    /// <param name="radius">radius of the nosecone</param>
    /// <param name="height">height of the nosecone</param>
    /// <param name="resolution">higher number is more circular</param>
    /// <param name="segments">higher number is more smooth</param>
    /// <returns>Elliptical Nosecone</returns>
    public EllipticalNosecone(double radius, double height, int resolution = 8, int segments = 8) {
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate() {
        return new ListMesh(
            FuncNosecone(
                (h) => {
                    h = Math.Max(0, Math.Min(h, height)); // clamp h between 0 and height
                    //var hh = h * h;
                    var hShifted = h - height;
                    var hShifted2 = hShifted * hShifted;
                    var height2 = height * height;
                    var flipped = (radius/height) * Math.Sqrt( (height2) - (hShifted2));
                    return flipped;
                    //var hOverHeight = Math.Min( ((hh) / (height2)), 1 ); // clamp ratio to < 1
                    //return -radius * Math.Sqrt(1 - hOverHeight) + radius;
                },
                radius, height, resolution, segments
            )
        );
    }
    
}

public class ParabolicNosecone : Nosecone {

    double K; double radius; double height; int resolution = 8; int segments = 8;
    public double ParabolicParametre {
        get => K;
        set { 
            K = value > 1 ? 1 : (value < 0 ? 0 : value); // clamp K
            Rebuild(); 
        }
    }
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
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
    public ParabolicNosecone(double K, double radius, double height, int resolution = 8, int segments = 8) {
        this.ParabolicParametre = K;
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate () {
        return new ListMesh(
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
}

public class PowerseriesNosecone : Nosecone {
    double n; double radius; double height; int resolution = 8; int segments = 8;

    public double PowerseriesParametre {
        get => n;
        set { 
            n = value > 1 ? 1 : (value < 0 ? 0 : value); // clamp n
            Rebuild();
        }
    }
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
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
    public PowerseriesNosecone (double n, double radius, double height, int resolution = 8, int segments = 8) {
        this.PowerseriesParametre = n;
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate () {
        return new ListMesh(
            FuncNosecone(
                (h) => {
                    return radius * Math.Pow(h / height, n);
                },
                radius, height, resolution, segments
            )
        );
    }
}

public class HaackNosecone : Nosecone {

    double C; double radius; double height; int resolution = 8; int segments = 8;

    public double HaackParametre {
        get => C;
        set {
            C = value > 1 ? 1 : (value < 0 ? 0 : value); 
            Rebuild();
        }
    }
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }
    public double Height {
        get => height;
        set { height = value; Rebuild(); }
    }
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
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
    public HaackNosecone (double C, double radius, double height, int resolution = 8, int segments = 8) {
        this.HaackParametre = C;
        this.radius = radius;
        this.height = height;
        this.resolution = resolution;
        this.segments = segments;
    }

    protected override IMesh Generate () {
        var sqrtPi = Math.Sqrt(Math.PI);

        return new ListMesh(
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