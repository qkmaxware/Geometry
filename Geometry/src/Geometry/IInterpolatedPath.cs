using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Curve which can be interpolated over in 2D
/// </summary>
public interface IInterpolatedPath2 {
    /// <summary>
    /// Position on the curve at the given distance
    /// </summary>
    Vec2 this[double distance] {get;}
}

/// <summary>
/// Curve which can be interpolated over in 3D
/// </summary>
public interface IInterpolatedPath3 {
    /// <summary>
    /// Position on the curve at the given distance
    /// </summary>
    Vec3 this[double distance] {get;}
}

}