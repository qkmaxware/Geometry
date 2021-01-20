using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Curve which can be interpolated over in 2D
/// </summary>
public interface IInterpolatedPath2 {
    /// <summary>
    /// Starting position
    /// </summary>
    Vec2 Start {get;}
    /// <summary>
    /// Ending postion
    /// </summary>
    Vec2 End {get;}

    /// <summary>
    /// Position on the curve at the given interpolation point
    /// </summary>
    Vec2 this[double t] {get;}
}

/// <summary>
/// Curve which can be interpolated over in 3D
/// </summary>
public interface IInterpolatedPath3 {
    /// <summary>
    /// Starting position
    /// </summary>
    Vec3 Start {get;}
    /// <summary>
    /// Ending postion
    /// </summary>
    Vec3 End {get;}

    /// <summary>
    /// Position on the curve at the given interpolation point
    /// </summary>
    Vec3 this[double t] {get;}

    /// <summary>
    /// Tangent vector at the given interpolation point
    /// </summary>
    /// <param name="t">interpolation variable</param>
    /// <returns>tangent vector</returns>
    Vec3 Tangent(double t);
}

}