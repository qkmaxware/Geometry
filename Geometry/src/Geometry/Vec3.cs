using System;
using System.Linq;

namespace Qkmaxware.Geometry {

/// <summary>
/// Represents a vector with double presision in 3d space
/// </summary>
public class Vec3 {
    /// <summary>
    /// X Coordinate 
    /// </summary>
    public double X {get; private set;}
    /// <summary>
    /// Y Coordinate 
    /// </summary>
    public double Y {get; private set;}
    /// <summary>
    /// Z Coordinate 
    /// </summary>
    public double Z {get; private set;}

    /// <summary>
    /// XY subset
    /// </summary>
    public Vec2 XY => new Vec2(X, Y);
    /// <summary>
    /// YX subset
    /// </summary>
    public Vec2 YX => new Vec2(Y, X);
    /// <summary>
    /// XZ subset
    /// </summary>
    public Vec2 XZ => new Vec2(X, Z);
    /// <summary>
    /// ZX subset
    /// </summary>
    public Vec2 ZX => new Vec2(Z, X);
    /// <summary>
    /// YZ subset
    /// </summary>
    public Vec2 YZ => new Vec2(Y, Z);
    /// <summary>
    /// ZY subset
    /// </summary>
    public Vec2 ZY => new Vec2(Z, Y);

    /// <summary>
    /// Vector at the world origin
    /// </summary>
    public static readonly Vec3 Zero = new Vec3(0, 0, 0);
    /// <summary>
    /// A vector composed of all 1's
    /// </summary>
    public static readonly Vec3 One = new Vec3(1, 1, 1);
    /// <summary>
    /// World X axis unit vector
    /// </summary>
    public static readonly Vec3 I = new Vec3(1, 0, 0);
    /// <summary>
    /// World Y axis unit vector
    /// </summary>
    public static readonly Vec3 J = new Vec3(0, 1, 0);
    /// <summary>
    /// World Z axis unit vector
    /// </summary>
    public static readonly Vec3 K = new Vec3(0, 0, 1);

    /// <summary>
    /// Create a new vector 
    /// </summary>
    /// <param name="x">x coordinate</param>
    public Vec3 (double x) : this(x, 0, 0) {}
    /// <summary>
    /// Create a new vector 
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public Vec3 (double x, double y) : this(x, y, 0) {}
    /// <summary>
    /// Create a new vector 
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    public Vec3 (double x, double y, double z) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Get a component of this vector (0 indexed)
    /// </summary>
    public double this[int index] {
        get {
            return (index % 3) switch {
                0 => X,
                1 => Y,
                2 => Z,
                _ => 0
            };
        }
    }

    /// <summary>
    /// Length of the vector
    /// </summary>
    public double Length => System.Math.Sqrt(SqrLength);
    /// <summary>
    /// Squared length of the vector
    /// </summary>
    public double SqrLength => (X * X + Y * Y + Z * Z);
    /// <summary>
    /// Vector in the same direction but of unit length
    /// </summary>
    public Vec3 Normalized {
        get {
            double length = Length;
            if (length == 0) {
                throw new DivideByZeroException();
            }
            double invLength = 1 / length;
            return new Vec3(X * invLength, Y * invLength, Z * invLength);
        }
    }
    /// <summary>
    /// Vector with the absolute value of all components
    /// </summary>
    public Vec3 Abs => new Vec3(System.Math.Abs(X), System.Math.Abs(Y), System.Math.Abs(Z));
    /// <summary>
    /// Vector flipped in the opposite direction
    /// </summary>
    public Vec3 Flipped => -1 * this;

    /// <summary>
    /// Returns an arbitrary vector orthogonal to this one
    /// </summary>
    /// <value>orthogonal vector</value>
    public Vec3 Orthogonal {
        get {
            var x = Math.Abs(this.X);
            var y = Math.Abs(this.Y);
            var z = Math.Abs(this.Z);

            var other = x < y ? (x < z ? Vec3.I : Vec3.K) : (y < z ? Vec3.J : Vec3.K);
            return Vec3.Cross(this, other);
        }
    }

    /// <summary>
    /// Max component of this vector
    /// </summary>
    public double Max() {
        return Math.Max(X, Math.Max(Y, Z));
    }

    /// <summary>
    /// Compute a vector with the maximal components of two input vectors
    /// </summary>
    /// <param name="vectors">list of vectors</param>
    /// <returns>new vector will the max components of the two inputs</returns> 
    public static Vec3 Max(params Vec3[] vectors) {
        return new Vec3(
            vectors.Select(v => v.X).Max(),
            vectors.Select(v => v.Y).Max(),
            vectors.Select(v => v.Z).Max()
        );
    }

    /// <summary>
    /// Minimum component of this vector
    /// </summary>
    public double Min() {
        return Math.Min(X, Math.Min(Y, Z));
    }

    /// <summary>
    /// Compute a vector with the minimal components of two input vectors
    /// </summary>
    /// <param name="vectors">list of vectors</param>
    /// <returns>new vector will the min components of the two inputs</returns>
    public static Vec3 Min(params Vec3[] vectors) {
        return new Vec3(
            vectors.Select(v => v.X).Min(),
            vectors.Select(v => v.Y).Min(),
            vectors.Select(v => v.Z).Min()
        );
    }

    /// <summary>
    /// Dot product of two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>dot product</returns>
    public static double Dot (Vec3 lhs, Vec3 rhs) {
        return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
    }

    /// <summary>
    /// Cross product of two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>cross product</returns>
    public static Vec3 Cross (Vec3 lhs, Vec3 rhs) {
        return new Vec3(
            lhs.Y * rhs.Z - lhs.Z * rhs.Y,
            lhs.Z * rhs.X - lhs.X * rhs.Z,
            lhs.X * rhs.Y - lhs.Y * rhs.X
        );
    }

    /// <summary>
    /// Distance between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>distance</returns>
    public static double Distance (Vec3 lhs, Vec3 rhs) {
        return (rhs - lhs).Length;
    }

    /// <summary>
    /// Squared distance between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>squared distance</returns>
    public static double SqrDistance (Vec3 lhs, Vec3 rhs) {
        return (rhs - lhs).SqrLength;
    }

    /// <summary>
    /// Linear interpolate between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <param name="t">interpolation parametre</param>
    /// <returns>vector interpolated along the path between the two points</returns>
    public static Vec3 Lerp (Vec3 lhs, Vec3 rhs, double t) {
        return (1 - t) * lhs + t * rhs;
    }

    /// <summary>
    /// Angle between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>angle</returns>
    public static double Angle (Vec3 lhs, Vec3 rhs) {
        return System.Math.Acos(
            Dot(lhs, rhs) / (lhs.Length * rhs.Length)
        );
    }   

    /// <summary>
    /// Signed angle between two vectors
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <param name="axis">axis to use when determining sign</param>
    /// <returns>signed angle between two vector around the given axis</returns>
    public static double SignedAngle(Vec3 lhs, Vec3 rhs, Vec3 axis) {
        var angle = Angle(lhs, rhs);
        var cross = Cross(lhs, rhs);
        if (Dot(axis, cross) < 0) { // Or > 0
            angle = -angle;
        }
        return angle;
    }

    /// <summary>
    /// Reflect this vector around another
    /// </summary>
    /// <param name="normal">vector reflection line</param>
    /// <returns>reflected vector</returns>
    public Vec3 Reflect(Vec3 normal) {
        Vec3 vertical = this - this.VectorProjectionOnto(normal);
        return this - 2 * vertical;
        //return this - 2 * this.VectorProjectionOnto(normal);
    }

    /// <summary>
    /// Project this vector onto another
    /// </summary>
    /// <param name="normal">projection vector</param>
    /// <returns>projected vector</returns>
    public Vec3 VectorProjectionOnto(Vec3 normal) {
        double mag = normal.SqrLength;
        return normal * (Vec3.Dot(this, normal) / (mag));
    }

    /// <summary>
    /// Project this vector onto another
    /// </summary>
    /// <param name="normal">projection vector</param>
    /// <returns>scalar projection value</returns>
    public double ScalarProjectionOnto(Vec3 normal) {
        double m = normal.SqrLength;
        return Vec3.Dot(this, normal) / (m);
    }

    /// <summary>
    /// Vector sum
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>vector sum</returns>
    public static Vec3 operator + (Vec3 lhs, Vec3 rhs) {
        return new Vec3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    /// <summary>
    /// Vector difference
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">second vector</param>
    /// <returns>vector difference</returns>
    public static Vec3 operator - (Vec3 lhs, Vec3 rhs) {
        return new Vec3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    /// <summary>
    /// Scalar multiplication
    /// </summary>
    /// <param name="lhs">scale value</param>
    /// <param name="rhs">second vector</param>
    /// <returns>Scalar multiplication</returns>
    public static Vec3 operator * (double lhs, Vec3 rhs) {
        return new Vec3(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);
    }

    /// <summary>
    /// Scalar multiplication
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">scale value</param>
    /// <returns>Scalar multiplication</returns>
    public static Vec3 operator * (Vec3 lhs, double rhs) {
        return new Vec3(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    /// <summary>
    /// Scalar division
    /// </summary>
    /// <param name="lhs">first vector</param>
    /// <param name="rhs">scale value</param>
    /// <returns>Scalar division</returns>
    public static Vec3 operator / (Vec3 lhs, double rhs) {
        return new Vec3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
    }

    /// <summary>
    /// Vector negation
    /// </summary>
    /// <param name="value">first vector</param>
    /// <returns>vector in the opposite direction</returns>
    public static Vec3 operator - (Vec3 value) {
        return new Vec3(-value.X, -value.Y, -value.Z);
    }   

    /// <summary>
    /// Convert vector to string
    /// </summary>
    /// <returns>string representation of the vector</returns>
    public override string ToString() {
        return string.Format("(x:{0:0.000},y:{1:0.000},z:{2:0.000})", X, Y, Z);
    }

    /// <summary>
    /// Vector equality check
    /// </summary>
    /// <param name="obj">object to check against</param>
    /// <returns>true if the object is a vector and the components match</returns>
    public override bool Equals(object obj) {
        return obj switch {
            Vec3 other => this.X == other.X && this.Y == other.Y && this.Z == other.Z,
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return System.HashCode.Combine(this.X, this.Y, this.Z);
    }
}

}