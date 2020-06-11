using System;
using System.Linq;

namespace Qkmaxware.Geometry {

/// <summary>
/// 3d Vector
/// </summary>
public class Vec2 : IEquatable<Vec2> {
    /// <summary>
    /// Zero vector
    /// </summary>
    /// <returns></returns>
    public static readonly Vec2 Zero = new Vec2(0,0);
    /// <summary>
    /// Vector with all components set to 1
    /// </summary>
    /// <returns></returns>
    public static readonly Vec2 One = new Vec2(1,1);
    /// <summary>
    /// Vector along the X axis
    /// </summary>
    /// <returns></returns>
    public static readonly Vec2 I = new Vec2(1,0);
    /// <summary>
    /// Vector along the Y axis
    /// </summary>
    /// <returns></returns>
    public static readonly Vec2 J = new Vec2(0,1);

    /// <summary>
    /// X component
    /// </summary>
    /// <value>double</value>
    public double X {get; private set;}
    /// <summary>
    /// Y component
    /// </summary>
    /// <value>double</value>
    public double Y {get; private set;}

    /// <summary>
    /// Squared length of the vector
    /// </summary>
    /// <value>double</value>
    public double SqrLength {
        get {
            return X * X + Y * Y;
        }
    }

    /// <summary>
    /// Length of the vector
    /// </summary>
    /// <value>double</value>
    public double Length {
        get {
            return System.Math.Sqrt(SqrLength);
        }
    }

    /// <summary>
    /// Vector in the same direction, but of unit length
    /// </summary>
    /// <value>unit vector</value>
    public Vec2 Normalized {
        get {
            double m = Length;
            if (m == 0) {
                throw new DivideByZeroException();
            }
            return new Vec2(X / m, Y / m);
        }
    }

    /// <summary>
    /// Return the negated version of this vector
    /// </summary>
    /// <value>negated vector</value>
    public Vec2 Flipped {
        get {
            return new Vec2(-X, -Y);
        }
    }

    /// <summary>
    /// Absolute value of all vector components
    /// </summary>
    /// <value>vector</value>
    public Vec2 Abs {
        get {
            return new Vec2(System.Math.Abs(X),System.Math.Abs(Y));
        }
    }

    /// <summary>
    /// Construct a new vector 
    /// </summary>
    public Vec2() {
        this.X = 0;
        this.Y = 0;
    }

    /// <summary>
    /// Construct a new vector 
    /// </summary>
    /// <param name="x">x component</param>
    public Vec2(double x) {
        this.X = x;
        this.Y = 0;
    }

    /// <summary>
    /// Construct a new vector 
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    public Vec2(double x, double y) {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Compare two vectors
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Vec2 other) {
        return X==(other.X) && Y==(other.Y);
    }

    /// <summary>
    /// Compare to object
    /// </summary>
    /// <param name="other">other object</param>
    /// <returns>true of objects are equal</returns>
    public override bool Equals(object other) {
        if (other is Vec2) {
            return this.Equals((Vec2)other);
        } else {
            return base.Equals(other);
        }
    }

    /// <summary>
    /// Get unique hashcode
    /// </summary>
    /// <returns>hashcode</returns>
    public override int GetHashCode() {
        return System.HashCode.Combine(X, Y);
    }

    /// <summary>
    /// String representation of this vector
    /// </summary>
    /// <returns>string</returns>
    public override string ToString() {
        return string.Format("(x:{0:0.000},y:{1:0.000})", X, Y);
    }

    /// <summary>
    /// Reflect this vector around another
    /// </summary>
    /// <param name="normal">vector reflection line</param>
    /// <returns>reflected vector</returns>
    public Vec2 Reflect(Vec2 normal) {
        Vec2 vertical = this - this.VectorProjectionOnto(normal);
        return this - 2 * vertical;
        //return this - 2 * this.VectorProjectionOnto(normal);
    }

    /// <summary>
    /// Project this vector onto another
    /// </summary>
    /// <param name="normal">projection vector</param>
    /// <returns>projected vector</returns>
    public Vec2 VectorProjectionOnto(Vec2 normal) {
        double mag = normal.SqrLength;
        return normal * (Dot(this, normal) / (mag));
    }

    /// <summary>
    /// Project this vector onto another
    /// </summary>
    /// <param name="normal">projection vector</param>
    /// <returns>scalar projection value</returns>
    public double ScalarProjectionOnto(Vec2 normal) {
        double m = normal.SqrLength;
        return Dot(this, normal) / (m);
    }

    /// <summary>
    /// Lerp values
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static double Lerp(double a, double b, double t) {
        return (1 - t) * a + t * b;
    }

    /// <summary>
    /// Linearly interpolate between two vectors
    /// </summary>
    /// <param name="a">first vector</param>
    /// <param name="b">second vector</param>
    /// <param name="t">control</param>
    /// <returns>interpolated vector</returns>
    public static Vec2 Lerp(Vec2 a, Vec2 b, double t) {
        return new Vec2(
            Lerp(a.X, b.X, t),
            Lerp(a.Y, b.Y, t)
        );
    }

        /// <summary>
    /// Angle between two vectors
    /// </summary>
    /// <param name="a">first vector</param>
    /// <param name="b">second vector</param>
    /// <returns>angle in radians between both vectors</returns>
    public static double Angle(Vec2 a, Vec2 b) {
        return System.Math.Acos(
            Dot(a.Normalized, b.Normalized)
        );
    }

    /// <summary>
    /// Squared distance between two vectors
    /// </summary>
    /// <param name="from">starting vector</param>
    /// <param name="to">end vector</param>
    /// <returns>distance</returns>
    public static double SqrDistance(Vec2 from, Vec2 to) {
        var dx = from.X - to.X;
        var dy = from.Y - to.Y;
        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Distance between two vectors
    /// </summary>
    /// <param name="from">starting vector</param>
    /// <param name="to">end vector</param>
    /// <returns>distance</returns>
    public static double Distance(Vec2 from, Vec2 to) {
        return System.Math.Sqrt(SqrDistance(from, to));
    }

    /// <summary>
    /// Dot product between two vectors
    /// </summary>
    /// <param name="a">first vector</param>
    /// <param name="b">second vector</param>
    /// <returns>dot product</returns>
    public static double Dot(Vec2 a, Vec2 b) {
        return a.X * b.X + a.Y * b.Y;
    }

    /// <summary>
    /// Multiply two vectors component wise
    /// </summary>
    /// <param name="a">first vector</param>
    /// <param name="b">second vector</param>
    /// <returns>vector whose elements are the component wise multiplications of the input vectors</returns>
    public static Vec2 ScalarMultiply(Vec2 a, Vec2 b) {
        return new Vec2(a.X * b.X, a.Y * b.Y);
    }

    /// <summary>
    /// Compute a vector composed of the maximal elements from all input vectors
    /// </summary>
    /// <param name="vectors">list of vectors to extract the maximum from</param>
    /// <returns>max vector</returns>
    public static Vec2 Max(params Vec2[] vectors) {
        return new Vec2(
            vectors.Select(v => v.X).Max(),
            vectors.Select(v => v.Y).Max()
        );
    }

    /// <summary>
    /// Compute a vector composed of the minimal elements from all input vectors
    /// </summary>
    /// <param name="vectors">list of vectors to extract the minimum from</param>
    /// <returns>min vector</returns>
    public static Vec2 Min(params Vec2[] vectors) {
        return new Vec2(
            vectors.Select(v => v.X).Min(),
            vectors.Select(v => v.Y).Min()
        );
    }


    /// <summary>
    /// Add two vectors
    /// </summary>
    /// <param name="a">first vector</param>
    /// <param name="b">second vector</param>
    /// <returns>vector sum</returns>
    public static Vec2 operator + (Vec2 a, Vec2 b) {
        return new Vec2(
            a.X + b.X,
            a.Y + b.Y
        );
    }

    /// <summary>
    /// Subtract second vector from first
    /// </summary>
    /// <param name="a">second vector</param>
    /// <param name="b">first vector</param>
    /// <returns>vector difference</returns>
    public static Vec2 operator - (Vec2 a, Vec2 b) {
        return new Vec2(
            a.X - b.X,
            a.Y - b.Y
        );
    }

    /// <summary>
    /// Unitary negation
    /// </summary>
    /// <param name="a">vector to negate</param>
    /// <returns>negated vector</returns>
    public static Vec2 operator - (Vec2 a) {
        return a.Flipped;
    }

    /// <summary>
    /// Scale a vector by a scalar value
    /// </summary>
    /// <param name="s">scalar length</param>
    /// <param name="a">vector</param>
    /// <returns>scaled vector</returns>
    public static Vec2 operator * (double s, Vec2 a) {
        return new Vec2(
            a.X * s,
            a.Y * s
        );
    }

    /// <summary>
    /// Scale a vector by a scalar value
    /// </summary>
    /// <param name="s">scalar length</param>
    /// <param name="a">vector</param>
    /// <returns>scaled vector</returns>
    public static Vec2 operator * (Vec2 a, double s) {
        return new Vec2(
            a.X * s,
            a.Y * s
        );
    }

    /// <summary>
    /// Scale a vector by a scalar value
    /// </summary>
    /// <param name="s">scalar length</param>
    /// <param name="a">vector</param>
    /// <returns>scaled vector</returns>
    public static Vec2 operator / (Vec2 a, double s) {
        if (s == 0) {
            throw new DivideByZeroException();
        }
        return new Vec2(
            a.X / s,
            a.Y / s
        );
    }
}

}