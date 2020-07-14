using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Line in 3d space represented by specific endpoints
/// </summary>
public class Line3 : Tuple<Vec3, Vec3>, IEquatable<Line3>, IInterpolatedPath3 {
 
    public Line3(Vec3 a, Vec3 b) : base(a,b) {}
 
    /// <summary>
    /// Edge from start to end
    /// </summary>
    /// <value>vector</value>
    public Vec3 Edge12 {
        get {
            return this.Item2 - this.Item1;
        }
    }

    /// <summary>
    /// Edge from end to start
    /// </summary>
    /// <value>vector</value>
    public Vec3 Edge21 {
        get {
            return this.Item1 - this.Item2;
        }
    }

    /// <summary>
    /// Squared length of line segment
    /// </summary>
    /// <value>squared length</value>
    public double SqrLength {
        get {
            return (Item1 - Item2).SqrLength;
        }
    }
 
    /// <summary>
    /// Length of line segment
    /// </summary>
    /// <value>length</value>
    public double Length {
        get {
            return (Item1 - Item2).Length;
        }
    }

    /// <summary>
    /// Position on the curve at the given distance
    /// </summary>
    public Vec3 this[double distance] => Item1 + distance * Edge12;

    /// <summary>
    /// Compare two line segments
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Line3 other) {
        return (
            (this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2)) ||
            (this.Item2.Equals(other.Item1) && this.Item1.Equals(other.Item2))
        );
    }

    private double Clamp01(double x) {
        if (x > 1)
            return 1;
        else if (x < 0)
            return 0;
        else
            return x;
    }

    /// <summary>
    /// Closest point on this line to the given point
    /// </summary>
    /// <param name="position">point</param>
    /// <returns>closest point</returns>
    public Vec3 ClosestPointTo(Vec3 position) {
        Vec3 a = this.Item1;
        Vec3 b = this.Item2;

        // Project position onto ab
        double t = Vec3.Dot(position - a, this.Edge12) / Vec3.Dot(this.Edge12, this.Edge12);

        // Compute the point
        return a + Clamp01(t) * this.Edge12;
    }

    public static explicit operator Ray(Line3 v) {
        return new Ray(v.Item1, v.Edge12);
    }

    public override string ToString() {
        return String.Format("(start:{0},end:{1})", Item1, Item2);
    }

}
 
}