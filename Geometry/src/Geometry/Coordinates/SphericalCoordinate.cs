using System;

namespace Qkmaxware.Geometry.Coordinates {

public class SphericalCoordinate {
    public double Distance {get; private set;}
    public double PolarAngle {get; private set;}
    public double AzimuthalAngle {get; private set;}

    public SphericalCoordinate() : this(0,0,0) {}
    public SphericalCoordinate(double distance, double polar, double azimuth) {
        this.Distance = distance;
        this.PolarAngle = polar;
        this.AzimuthalAngle = azimuth;
    }

    public static implicit operator Vec3 (SphericalCoordinate coord) {
        var r = coord.Distance;
        var SinTheta = Math.Sin(coord.PolarAngle);
        var CosTheta = Math.Cos(coord.PolarAngle);
        var CosPhi = Math.Cos(coord.AzimuthalAngle);
        var SinPhi = Math.Sin(coord.AzimuthalAngle);

        var x = r * SinTheta * CosPhi;
        var y = r * SinTheta * SinPhi;
        var z = r * CosTheta;

        return new Vec3(x, y, z);
    }

    public static implicit operator SphericalCoordinate (Vec3 coord) {
        var r = coord.Length;
        var theta = Math.Acos(coord.Z / r);
        var phi = Math.Atan2(coord.Y, coord.X);

        return new SphericalCoordinate(r, theta, phi);
    }
}

}