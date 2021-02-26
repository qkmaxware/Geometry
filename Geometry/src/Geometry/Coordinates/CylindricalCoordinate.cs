using System;

namespace Qkmaxware.Geometry.Coordinates {

public class CylindricalCoordinate {

    public double Distance {get; private set;}
    public double AzimuthalAngle {get; private set;}
    public double Altitude {get; private set;}

    public CylindricalCoordinate() : this(0,0,0) {}

    public CylindricalCoordinate(double distance, double azimuth, double height) {
        this.Distance = distance;
        this.AzimuthalAngle = azimuth;
        this.Altitude = height;
    }

    public static implicit operator Vec3 (CylindricalCoordinate coord) {
        var r = coord.Distance;
        var CosPhi = Math.Cos(coord.AzimuthalAngle);
        var SinPhi = Math.Sin(coord.AzimuthalAngle);

        var x = r * CosPhi;
        var y = r * SinPhi;
        var z = coord.Altitude;

        return new Vec3(x, y, z);
    }

    public static implicit operator CylindricalCoordinate (Vec3 coord) {
        var r = Math.Sqrt(coord.X * coord.X + coord.Y * coord.Y);
        var phi = Math.Atan2(coord.Y, coord.X);
        var z = coord.Z;

        return new CylindricalCoordinate(r, phi, z);
    }

}

}