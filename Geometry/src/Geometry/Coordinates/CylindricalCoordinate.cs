using System;

namespace Qkmaxware.Geometry.Coordinates {

/// <summary>
/// 3D vector in cylindrical coordinates
/// </summary>
public class CylindricalCoordinate {

    /// <summary>
    /// Distance from the origin
    /// </summary>
    /// <value>distance</value>
    public double Distance {get; private set;}
    /// <summary>
    /// Azimuthal angle around the vertical axis
    /// </summary>
    /// <value>angle</value>
    public double AzimuthalAngle {get; private set;}
    /// <summary>
    /// Height of the point
    /// </summary>
    /// <value>height</value>
    public double Altitude {get; private set;}

    /// <summary>
    /// Create an zero coordinate point
    /// </summary>
    /// <returns>cylindrical coordinate</returns>
    public CylindricalCoordinate() : this(0,0,0) {}

    /// <summary>
    /// Create a specific coordinate point
    /// </summary>
    /// <param name="distance">distance</param>
    /// <param name="azimuth">azimuthal angle</param>
    /// <param name="height">height</param>
    public CylindricalCoordinate(double distance, double azimuth, double height) {
        this.Distance = distance;
        this.AzimuthalAngle = azimuth;
        this.Altitude = height;
    }

    /// <summary>
    /// Convert a cylindrical coordinate to a cartesian one
    /// </summary>
    /// <param name="coord">cylindrical coordinate</param>
    public static implicit operator Vec3 (CylindricalCoordinate coord) {
        var r = coord.Distance;
        var CosPhi = Math.Cos(coord.AzimuthalAngle);
        var SinPhi = Math.Sin(coord.AzimuthalAngle);

        var x = r * CosPhi;
        var y = r * SinPhi;
        var z = coord.Altitude;

        return new Vec3(x, y, z);
    }

    /// <summary>
    /// Convert a cartesian coordinate to a cylindrical one
    /// </summary>
    /// <param name="coord">cartesian coordinate</param>
    public static implicit operator CylindricalCoordinate (Vec3 coord) {
        var r = Math.Sqrt(coord.X * coord.X + coord.Y * coord.Y);
        var phi = Math.Atan2(coord.Y, coord.X);
        var z = coord.Z;

        return new CylindricalCoordinate(r, phi, z);
    }

}

}