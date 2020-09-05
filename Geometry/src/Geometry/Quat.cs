using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// A rotation represented as a 4D vector or quaternion
/// </summary>
public class Quat {
    public static readonly Quat Identity = new Quat(0, 0, 0, 1);

    // ---------------------------------------------------------------------
    // 1D Subsets
    // ---------------------------------------------------------------------

    /// <summary>
    /// X Component 
    /// </summary>
    public double X {get; private set;}
    /// <summary>
    /// Y Component 
    /// </summary>
    public double Y {get; private set;}
    /// <summary>
    /// Z Component 
    /// </summary>
    public double Z {get; private set;}
    /// <summary>
    /// W Component 
    /// </summary>
    public double W {get; private set;}

    // ---------------------------------------------------------------------
    // 3D Subsets
    // ---------------------------------------------------------------------

    /// <summary>
    /// XYZ subset
    /// </summary>
    public Vec3 XYZ => new Vec3(X, Y, Z);
    /// <summary>
    /// XYW subset
    /// </summary>
    public Vec3 XYW => new Vec3(X, Y, W);
    /// <summary>
    /// XZY subset
    /// </summary>
    public Vec3 XZY => new Vec3(X, Z, Y);
    /// <summary>
    /// XZW subset
    /// </summary>
    public Vec3 XZW => new Vec3(X, Z, W);
    /// <summary>
    /// XWY subset
    /// </summary>
    public Vec3 XWY => new Vec3(X, W, Y);
    /// <summary>
    /// XWZ subset
    /// </summary>
    public Vec3 XWZ => new Vec3(X, W, Z);
    /// <summary>
    /// YXZ subset
    /// </summary>
    public Vec3 YXZ => new Vec3(Y, X, Z);
    /// <summary>
    /// YXW subset
    /// </summary>
    public Vec3 YXW => new Vec3(Y, X, W);
    /// <summary>
    /// YZX subset
    /// </summary>
    public Vec3 YZX => new Vec3(Y, Z, X);
    /// <summary>
    /// YZW subset
    /// </summary>
    public Vec3 YZW => new Vec3(Y, Z, W);
    /// <summary>
    /// YWX subset
    /// </summary>
    public Vec3 YWX => new Vec3(Y, W, X);
    /// <summary>
    /// YWZ subset
    /// </summary>
    public Vec3 YWZ => new Vec3(Y, W, Z);
    /// <summary>
    /// ZXY subset
    /// </summary>
    public Vec3 ZXY => new Vec3(Z, X, Y);
    /// <summary>
    /// ZXW subset
    /// </summary>
    public Vec3 ZXW => new Vec3(Z, X, W);
    /// <summary>
    /// ZYX subset
    /// </summary>
    public Vec3 ZYX => new Vec3(Z, Y, X);
    /// <summary>
    /// ZYW subset
    /// </summary>
    public Vec3 ZYW => new Vec3(Z, Y, W);
    /// <summary>
    /// ZWX subset
    /// </summary>
    public Vec3 ZWX => new Vec3(Z, W, X);
    /// <summary>
    /// ZWY subset
    /// </summary>
    public Vec3 ZWY => new Vec3(Z, W, Y);
    /// <summary>
    /// WXY subset
    /// </summary>
    public Vec3 WXY => new Vec3(W, X, Y);
    /// <summary>
    /// WXZ subset
    /// </summary>
    public Vec3 WXZ => new Vec3(W, X, Z);
    /// <summary>
    /// WYX subset
    /// </summary>
    public Vec3 WYX => new Vec3(W, Y, X);
    /// <summary>
    /// WYZ subset
    /// </summary>
    public Vec3 WYZ => new Vec3(W, Y, Z);
    /// <summary>
    /// WZX subset
    /// </summary>
    public Vec3 WZX => new Vec3(W, Z, X);
    /// <summary>
    /// WZY subset
    /// </summary>
    public Vec3 WZY => new Vec3(W, Z, Y);

    // ---------------------------------------------------------------------
    // 2D Subsets
    // ---------------------------------------------------------------------

    /// <summary>
    /// XY subset
    /// </summary>
    public Vec2 XY => new Vec2(X, Y);
    /// <summary>
    /// XZ subset
    /// </summary>
    public Vec2 XZ => new Vec2(X, Z);
    /// <summary>
    /// XW subset
    /// </summary>
    public Vec2 XW => new Vec2(X, W);
    /// <summary>
    /// YX subset
    /// </summary>
    public Vec2 YX => new Vec2(Y, X);
    /// <summary>
    /// YZ subset
    /// </summary>
    public Vec2 YZ => new Vec2(Y, Z);
    /// <summary>
    /// YW subset
    /// </summary>
    public Vec2 YW => new Vec2(Y, W);
    /// <summary>
    /// ZX subset
    /// </summary>
    public Vec2 ZX => new Vec2(Z, X);
    /// <summary>
    /// ZY subset
    /// </summary>
    public Vec2 ZY => new Vec2(Z, Y);
    /// <summary>
    /// ZW subset
    /// </summary>
    public Vec2 ZW => new Vec2(Z, W);
    /// <summary>
    /// WX subset
    /// </summary>
    public Vec2 WX => new Vec2(W, X);
    /// <summary>
    /// WY subset
    /// </summary>
    public Vec2 WY => new Vec2(W, Y);
    /// <summary>
    /// WZ subset
    /// </summary>
    public Vec2 WZ => new Vec2(W, Z);

    /// <summary>
    /// Create a new quaternion from the given components
    /// </summary>
    /// <param name="x">x component</param>
    /// <param name="y">y component</param>
    /// <param name="z">z component</param>
    /// <param name="w">w component</param>
    public Quat(double x = 0, double y = 0, double z = 0, double w = 1) {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    /// <summary>
    /// Create a quaternion from the vector components
    /// </summary>
    /// <param name="vec">XYZ components</param>
    /// <param name="w">w component</param>
    public Quat (Vec3 vec, double w) : this(vec.X, vec.Y, vec.Z, w) {}

    /// <summary>
    /// Copy an existing quaternion
    /// </summary>
    /// <param name="other">quaternion to copy</param>
    public Quat (Quat other) : this (other.X, other.Y, other.Z, other.W) {}

    /// <summary>
    /// Conjugate of this quaternion
    /// </summary>
    /// <returns>conjugate quaternion</returns>
    public Quat Conjugate => new Quat(-this.X, -this.Y, -this.Z, this.W);    

    /// <summary>
    /// Length of the quaternion vector
    /// </summary>
    public double Length => System.Math.Sqrt(SqrLength);

    /// <summary>
    /// Squared length of the quaternion vector
    /// </summary>
    public double SqrLength => (X * X + Y * Y + Z * Z + W * W);

    /// <summary>
    /// Quaternion vector in the same direction but of unit length
    /// </summary>
    public Quat Normalized {
        get {
            double length = Length;
            if (length == 0) {
                throw new DivideByZeroException();
            }
            double invLength = 1 / length;
            return new Quat(X * invLength, Y * invLength, Z * invLength, W * invLength);
        }
    }

    // <summary>
    /// Quaternion vector with the absolute value of all components
    /// </summary>
    public Quat Abs => new Quat(System.Math.Abs(X), System.Math.Abs(Y), System.Math.Abs(Z), System.Math.Abs(W));

    /// <summary>
    /// Quaternion vector flipped in the opposite direction
    /// </summary>
    public Quat Flipped => -1 * this;

    /// <summary>
    /// Dot product of two quaternion vectors
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <returns>vector dot product</returns>
    public static double Dot(Quat a, Quat b) {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
    }

    /// <summary>
    /// Angle between two quaternions
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <returns>angle</returns>
    public static double Angle (Quat a, Quat b) {
        var d = Dot(a, b);
        return Math.Acos(Math.Min(Math.Abs(d), 1) * 2);
    }

    /// <summary>
    /// Linearly interpolate between two quaternions
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <param name="t">interpolation factor</param>
    /// <returns>interpolated quaternion</returns>
    public static Quat Lerp(Quat a, Quat b, double t) {
        return (1 - t) * a + t * b;
    }

    /// <summary>
    /// Spherically interpolate between two quaternions
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <param name="t">interpolation factor</param>
    /// <returns>interpolated quaternion</returns>
    public static Quat Slerp(Quat a, Quat b, double t) {
        a = a.Normalized;
        b = b.Normalized;

        var dot = Dot(a, b);

        if (dot < 0) {
            b = -1 * b;
            dot = -dot;
        }

        double theta0 = Math.Acos(dot);
        double theta = theta0 * t;
        double sin_theta = Math.Sin(theta);
        double sin_theta0 = Math.Sin(theta0);

        double s0 = Math.Cos(theta) - dot * sin_theta / sin_theta0;
        double s1 = sin_theta / sin_theta0;

        return (s0 * a) + (s1 * b);
    }

    /// <summary>
    /// Create a quaternion from yaw-pitch-roll rotation angles
    /// </summary>
    /// <param name="angles">rotations around XYZ axes where </param>
    /// <returns>quaternion</returns>
    public static Quat YawPitchRoll(Vec3 angles) {
        var yaw = angles.Z;
        var pitch = angles.X;
        var roll = angles.Y;

        return YawPitchRoll(yaw, pitch, roll);
    }

    /// <summary>
    /// Create a quaternion from yaw-pitch-roll rotation angles
    /// </summary>
    /// <param name="yaw">yaw angle</param>
    /// <param name="pitch">pitch angle</param>
    /// <param name="roll">roll angle</param>
    /// <returns>quaternion</returns>
    public static Quat YawPitchRoll(double yaw, double pitch, double roll) {
        double cy = Math.Cos(yaw * 0.5);
        double sy = Math.Sin(yaw * 0.5);
        double cp = Math.Cos(pitch * 0.5);
        double sp = Math.Sin(pitch * 0.5);
        double cr = Math.Cos(roll * 0.5);
        double sr = Math.Sin(roll * 0.5);

        return new Quat(
            x: sr * cp * cy - cr * sp * sy,
            y: cr * sp * cy + sr * cp * sy,
            z: cr * cp * sy - sr * sp * cy,
            w: cr * cp * cy + sr * sp * sy
        );
    }

    /// <summary>
    /// Create a quaternion from an angle axis representation
    /// </summary>
    /// <param name="axis">axis</param>
    /// <param name="angle">angle</param>
    /// <returns>quaternion</returns>
    public static Quat AngleAxis(Vec3 axis, double angle) {
        if (axis.SqrLength == 0)
            return Identity;

        double rad = (angle) * 0.5;
        Vec3 normal = axis.Normalized;
        normal = normal * (Math.Sin(rad));
        return new Quat(
                normal.X,
                normal.Y,
                normal.Z,
                Math.Cos(rad)
        ).Normalized;
    }

    /// <summary>
    /// Create a quaternion representing a rotation looking in the desired direction
    /// </summary>
    /// <param name="direction">look direction</param>
    /// <returns>rotation</returns>
    public static Quat LookRotation(Vec3 direction) {
        return LookRotation(direction, Vec3.K);
    }

    /// <summary>
    /// Create a quaternion representing a rotation looking in the desired direction
    /// </summary>
    /// <param name="direction">look direction</param>
    /// <param name="upwards">planar normal direction</param>
    /// <returns>rotation</returns>
    public static Quat LookRotation(Vec3 direction, Vec3 upwards) {
        if (direction == Vec3.Zero)
            return Quat.Identity;
        
        if (upwards != direction) {
            upwards = upwards.Normalized;
            var v = direction + upwards * -Vec3.Dot(upwards, direction);
            var q = Quat.FromToRotation(Vec3.J, v);
            return Quat.FromToRotation(v, direction) * q;
        } else {
            return Quat.FromToRotation(Vec3.J, direction);
        }
    }

    /// <summary>
    /// Create a rotation from one vector to another
    /// </summary>
    /// <param name="u">first vector</param>
    /// <param name="v">second vector</param>
    /// <returns>rotation from the first to the second</returns>
    public static Quat FromToRotation(Vec3 u, Vec3 v) {
        u = u.Normalized;
        v = v.Normalized;
        
        var k_cos_theta =  Vec3.Dot(u,v);
        var k = Math.Sqrt(u.SqrLength * v.SqrLength);

        if (k_cos_theta / k == -1) {
            // 180 degree rotation on any orthogonal vector
            return new Quat(u.Orthogonal.Normalized, 0);
        } else {
            return new Quat(Vec3.Cross(u,v), k_cos_theta + k).Normalized;
        }
    }

    /// <summary>
    /// Sum of two quaternions
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <returns>sum</returns>
    public static Quat operator + (Quat a, Quat b) {
        return new Quat(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }

    /// <summary>
    /// Quaternion vector negation
    /// </summary>
    /// <param name="value">first vector</param>
    /// <returns>quaternion with all values negated</returns>
    public static Quat operator - (Quat value) {
        return new Quat(-value.X, -value.Y, -value.Z, -value.W);
    } 

    /// <summary>
    /// Difference of two quaternions
    /// </summary>
    /// <param name="a">first quaternion</param>
    /// <param name="b">second quaternion</param>
    /// <returns>difference</returns>
    public static Quat operator - (Quat a, Quat b) {
        return new Quat(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    }

    /// <summary>
    /// Scalar multiplication
    /// </summary>
    /// <param name="a">scalar value</param>
    /// <param name="b">quaternion</param>
    /// <returns>scalar multiplication</returns>
    public static Quat operator * (double a, Quat b) {
        return new Quat(a * b.X, a * b.Y, a * b.Z, a * b.W);
    }

    /// <summary>
    /// Scalar multiplication
    /// </summary>
    /// <param name="a">quaternion</param>
    /// <param name="b">scalar value</param>
    /// <returns>scalar multiplication</returns>
    public static Quat operator * (Quat a, double b) {
        return new Quat(a.X * b, a.Y * b, a.Z * b, a.W * b);
    }

    /// <summary>
    /// Scalar division
    /// </summary>
    /// <param name="a">quaternion</param>
    /// <param name="b">scalar value</param>
    /// <returns>scalar division</returns>
    public static Quat operator / (Quat a, double b) {
        return new Quat(a.X / b, a.Y / b, a.Z / b, a.W / b);
    }

    /// <summary>
    /// Multiply two quaternions
    /// </summary>
    /// <param name="lhs">first quaternion</param>
    /// <param name="rhs">second quaternion</param>
    /// <returns>pr</returns>
    public static Quat operator * (Quat lhs, Quat rhs) {
        var a = lhs.W;  var e = rhs.W;
        var b = lhs.X;  var f = rhs.X;
        var c = lhs.Y;  var g = rhs.Y;
        var d = lhs.Z;  var h = rhs.Z;

        var w = a*e - b*f - c*g- d*h;
        var x = (b*e + a*f + c*h - d*g);
        var y = (a*g - b*h + c*e + d*f);
        var z = (a*h + b*g - c*f + d*e);

        return new Quat(x, y, z, w);
    }

    /// <summary>
    /// Quaternion division
    /// </summary>
    /// <param name="lhs">first quaternion</param>
    /// <param name="rhs">second quaternion</param>
    /// <returns>division of the first by the second quaternion</returns>
    public static Quat operator / (Quat lhs, Quat rhs) {
        return lhs * (rhs.Conjugate.Normalized);
    }

    /// <summary>
    /// Implicitly convert a quaternion to a rotation matrix
    /// </summary>
    /// <param name="quat">quaternion to convert</param>
    public static implicit operator Transformation (Quat q) {
        double sqw = q.W*q.W;
        double sqx = q.X*q.X;
        double sqy = q.Y*q.Y;
        double sqz = q.Z*q.Z;

        double invs = 1 / (sqx + sqy + sqz + sqw);
        var m00 = ( sqx - sqy - sqz + sqw)*invs;
        var m11 = (-sqx + sqy - sqz + sqw)*invs;
        var m22 = (-sqx - sqy + sqz + sqw)*invs;

        double tmp1 = q.X*q.Y;
        double tmp2 = q.Z*q.W;
        var m10 = 2.0 * (tmp1 + tmp2)*invs;
        var m01 = 2.0 * (tmp1 - tmp2)*invs;

        tmp1 = q.X*q.Z;
        tmp2 = q.Y*q.W;
        var m20 = 2.0 * (tmp1 - tmp2)*invs;
        var m02 = 2.0 * (tmp1 + tmp2)*invs;
        tmp1 = q.Y*q.Z;
        tmp2 = q.X*q.W;
        var m21 = 2.0 * (tmp1 + tmp2)*invs;
        var m12 = 2.0 * (tmp1 - tmp2)*invs;

        return new Transformation(
            m00, m01, m02, 0,
            m10, m11, m12, 0,
            m20, m21, m22, 0
        );
    }

    /// <summary>
    /// Convert vector to string
    /// </summary>
    /// <returns>string representation of the vector</returns>
    public override string ToString() {
        return string.Format("(x:{0:0.000},y:{1:0.000},z:{2:0.000},w:{3:0.000})", X, Y, Z, W);
    }

    /// <summary>
    /// Vector equality check
    /// </summary>
    /// <param name="obj">object to check against</param>
    /// <returns>true if the object is a vector and the components match</returns>
    public override bool Equals(object obj) {
        return obj switch {
            Quat other => this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W,
            _ => base.Equals(obj)
        };
    }

    public override int GetHashCode() {
        return System.HashCode.Combine(this.X, this.Y, this.Z, this.W);
    }
}

}