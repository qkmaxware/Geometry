using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Line in 2d space represented by specific endpoints
/// </summary>
public class Line2 : Tuple<Vec2, Vec2>, IEquatable<Line2>, IInterpolatedPath2 {
 
    public Line2(Vec2 a, Vec2 b) : base(a,b) {}
 
    /// <summary>
    /// Edge from start to end
    /// </summary>
    /// <value>vector</value>
    public Vec2 Edge12 {
        get {
            return this.Item2 - this.Item1;
        }
    }

    /// <summary>
    /// Edge from end to start
    /// </summary>
    /// <value>vector</value>
    public Vec2 Edge21 {
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
    public Vec2 this[double distance] => Item1 + distance * Edge12;

    /// <summary>
    /// Compare two line segments
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Line2 other) {
        return (
            (this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2)) ||
            (this.Item2.Equals(other.Item1) && this.Item1.Equals(other.Item2))
        );
    }

    /// <summary>
    /// Check if these two lines intersect on infinity
    /// </summary>
    /// <param name="ln">line to intersect with</param>
    /// <param name="lerpThis">position of intersection along this line segment</param>
    /// <param name="lerpLn">position of intersection along the other line segment</param>
    /// <returns>position along the second line segment where the intersection took place</returns>
    public bool Intersects (Line2 ln, out double lerpThis, out double lerpLn) {
        // This = Item1 + t * (Item2 - Item1)  =  (x1, y1) -> (x2, y2)
        // Ln   = Item1 + u * (Item2 - Item1)  =  (x3, y3) -> (x4, y4)
        double x1 = this.Item1.X;
        double y1 = this.Item1.Y;
        double x2 = this.Item2.X;
        double y2 = this.Item2.Y;

        double x3 = ln.Item1.X;
        double y3 = ln.Item1.Y;
        double x4 = ln.Item2.X;
        double y4 = ln.Item2.Y;

        double demon = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (demon == 0) {
            lerpThis = double.NaN;
            lerpLn = double.NaN;
            return false;
        } else{
            // Calculate t
            double num1 = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
            lerpThis = (num1 / demon);
            // Calculate u 
            double num2 = (x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3);
            lerpLn = -(num2 / demon);
            return true;
        }
    }

    /// <summary>
    /// Check if these two lines intersect on infinity
    /// </summary>
    /// <param name="ln">line to intersect with</param>
    /// <param name="position">position of intersection if it occurs</param>
    /// <returns>true if intersection occurs</returns>
    public bool Intersects(Line2 ln, out Vec2 position) {
        double l; double t;
        if (Intersects(ln, out l, out t)) {
            position = Vec2.Lerp(ln.Item1, ln.Item2, t);
            return true;
        } else {
            position = ln.Item1;
            return false;
        }
    }

    /// <summary>
    /// Check if this line intersects another long only its declared segment
    /// </summary>
    /// <param name="ln">line to intersect with</param>
    /// <param name="position">position of intersection if it occurs</param>
    /// <returns>true if intersection occurs</returns>
    public bool IntersectsSegment(Line2 ln, Vec2 position) {
        double l; double t;
        if (Intersects(ln, out l, out t) && (t >=0  && t <= 1) && (l >=0  && l <= 1)) {
            // Intersects and in the range 0-1
            position = Vec2.Lerp(ln.Item1, ln.Item2, t);
            return true;
        } else {
            position = ln.Item1;
            return false;
        }
    }

    private static double Clamp01(double t) {
        if (t < 0) {
            return 0;
        } else if (t > 1) {
            return 1;
        } else {
            return t;
        }
    }

    /// <summary>
    /// Closest point on this line to the given point
    /// </summary>
    /// <param name="position">point</param>
    /// <returns>closest point</returns>
    public Vec2 ClosestPointTo(Vec2 position) {
        Vec2 a = this.Item1;
        Vec2 b = this.Item2;

        // Project position onto ab
        double t = Vec2.Dot(position - a, this.Edge12) / Vec2.Dot(this.Edge12, this.Edge12);

        // Compute the point
        return a + Clamp01(t) * this.Edge12;
    }

    public override string ToString() {
        return String.Format("(start:{0},end:{1})", Item1, Item2);
    }
}

}