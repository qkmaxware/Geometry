using System;

namespace Qkmaxware.Geometry.Coordinates {

/// <summary>
/// 3D vector in spherical coordinates
/// </summary>
public class SphericalCoordinate {
    /// <summary>
    /// Distance from the origin
    /// </summary>
    /// <value>distance</value>
    public double Distance {get; private set;}
    /// <summary>
    /// Polar angle off of the vertical axis
    /// </summary>
    /// <value>angle</value>
    public double PolarAngle {get; private set;}
    /// <summary>
    /// Azimuthal angle around the vertical axis
    /// </summary>
    /// <value>angle</value>
    public double AzimuthalAngle {get; private set;}

    /// <summary>
    /// Create a zero coordinate
    /// </summary>
    /// <returns>spherical coordinate</returns>
    public SphericalCoordinate() : this(0,0,0) {}
    /// <summary>
    /// Create a spherical coordinate
    /// </summary>
    /// <param name="distance">distance</param>
    /// <param name="polar">polar angle</param>
    /// <param name="azimuth">azimuthal angle</param>
    public SphericalCoordinate(double distance, double polar, double azimuth) {
        this.Distance = distance;
        this.PolarAngle = polar;
        this.AzimuthalAngle = azimuth;
    }

    /// <summary>
    /// Convert a spherical coordinate to a cartesian one
    /// </summary>
    /// <param name="coord">spherical coordinate</param>
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

    /// <summary>
    /// Convert a cartesian coordinate to a spherical one 
    /// </summary>
    /// <param name="coord">cartesian coordinate</param>
    public static implicit operator SphericalCoordinate (Vec3 coord) {
        var r = coord.Length;
        var theta = Math.Acos(coord.Z / r);
        var phi = Math.Atan2(coord.Y, coord.X);

        return new SphericalCoordinate(r, theta, phi);
    }
}

}