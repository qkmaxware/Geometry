using System;

namespace Qkmaxware.Geometry {

public class CubicBezierCurve : IInterpolatedPath3 {
    public Vec3 Item1 {get; private set;}
    public Vec3 Item2 {get; private set;}
    public Vec3 Control1 {get; private set;}
    public Vec3 Control2 {get; private set;}

    public CubicBezierCurve(Vec3 item1, Vec3 control1, Vec3 control2, Vec3 item2){
        this.Item1 = item1;
        this.Control1 = control1;
        this.Control2 = control2;
        this.Item2 = item2;
    }

    /// <summary>
    /// Starting position
    /// </summary>
    public Vec3 Start => Item1;
    /// <summary>
    /// Ending postion
    /// </summary>
    public Vec3 End => Item2;

    /// <summary>
    /// Position on the curve at the given interpolation point
    /// </summary>
    public Vec3 this[double t] {
        get {
            t = ((t > 1) ? 1 : (t < 0 ? 0 : t)); // Clamp 0 and 1
            var _t = (1 - t);
            // (1 - t)^3 * P0 + 3t(1-t)^2 * P1 + 3t^2 (1-t) * P2 + t^3 * P3
            return (_t * _t * _t) * Item1 + (3 * _t * _t * t) * Control1 + (3 * _t * t * t) * Control2 + (t * t * t) * Item2;
        }
    }

    /// <summary>
    /// Tangent vector at the given point
    /// </summary>
    /// <param name="t">interpolation parametre</param>
    /// <returns>tangent vector</returns>
    public Vec3 Tangent(double t) {
        t = ((t > 1) ? 1 : (t < 0 ? 0 : t)); // Clamp 0 and 1
        var _t = (1 - t);
        // -3(1-t)^2 * P0             + 3(1-t)^2 * P1            - 6t(1-t) * P1          - 3t^2 * P2            + 6t(1-t) * P2          + 3t^2 * P3 
        var r =-3 * (_t * _t) * Item1 + 3 * (_t * _t) * Control1 - 6 * t * _t * Control1 - 2 * t * t * Control2 + 6 * t * _t * Control2 + 3 * t * t * Item2;
        return r.Normalized;
    }

}

}