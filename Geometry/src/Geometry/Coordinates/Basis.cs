using System;

namespace Qkmaxware.Geometry.Coordinates {

/// <summary>
/// Basis vector set
/// </summary>
public class Basis {
    /// <summary>
    /// Local 'X' axis
    /// </summary>
    /// <value>Local 'X' axis</value>
    public Vec3 X {get; private set;}
    /// <summary>
    /// Local 'Y' axis
    /// </summary>
    /// <value>Local 'Y' axis</value>
    public Vec3 Y {get; private set;}
    /// <summary>
    /// Local 'Z' axis
    /// </summary>
    /// <value>Local 'Z' axis</value>
    public Vec3 Z {get; private set;}

    /// <summary>
    /// Create a new basis vector set with the standard X,Y,Z axis
    /// </summary>
    public Basis() {
        this.X = Vec3.I;
        this.Y = Vec3.J;
        this.Z = Vec3.K;
    }

    /// <summary>
    /// Create a basis vector set from the transformation
    /// </summary>
    /// <param name="transformation">transformation to use as a basis</param>
    public Basis(Transformation transformation) {
        this.X = new Vec3(transformation[0,0], transformation[1,0], transformation[2,0]);
        this.Y = new Vec3(transformation[0,1], transformation[1,1], transformation[2,1]);
        this.Z = new Vec3(transformation[0,2], transformation[1,2], transformation[2,2]);
    }

    /// <summary>
    /// Create a new basis vector set with the given axis, vectors should be orthogonal
    /// </summary>
    /// <param name="xb">local x axis</param>
    /// <param name="yb">local y axis</param>
    /// <param name="zb">local z axis</param>
    public Basis(Vec3 x, Vec3 y, Vec3 z) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}

}