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
    /// Position on the curve at the given distance
    /// </summary>
    public Vec3 this[double distance] {
        get {
            var t = distance / Vec3.Distance(this.Item1, this.Item2);
            t = ((t > 1) ? 1 : (t < 0 ? 0 : t)); // Clamp 0 and 1
            var _t = (1 - t);
            return (_t * _t * _t) * Item1 + (3 * _t * _t * t) * Control1 + (3 * _t * t * t) * Control2 + (t * t * t) * Item2;
        }
    }

}

}