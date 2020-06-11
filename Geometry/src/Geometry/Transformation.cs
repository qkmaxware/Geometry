using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Geometry{

/// <summary>
/// Affine transformation
/// </summary>
public class Transformation {
    private double[,] rotation; // 3x3 matrix
    private double[] position; // 3

    /// <summary>
    /// Index element from this transformation matrix
    /// </summary>
    public double this[int row, int col] {
        get {
            if (row < 3 && col < 3) {
                return rotation[row, col]; 
            } else if (row ==3 && col < 3) {
                return position[col];
            } else if (row == 3 && col == 3) {
                return 1;
            } else {
                return 0;
            }
        } set {
            if (row < 3 && col < 3) {
                rotation[row, col] = value; 
            } else if (row ==3 && col < 3) {
                position[col] = value;
            } 
        }
    }

    /// <summary>
    /// World X axis of this reference frame
    /// </summary>
    public Vec3 X => new Vec3(this[0,0], this[1,0], this[2,0]);
    /// <summary>
    /// World Y axis of this reference frame
    /// </summary>
    public Vec3 Y => new Vec3(this[0,1], this[1,1], this[2,1]);
    /// <summary>
    /// World Z axis of this reference frame
    /// </summary>
    public Vec3 Z => new Vec3(this[0,2], this[1,2], this[2,2]);
    /// <summary>
    /// World Position of this reference frame
    /// </summary>
    public Vec3 Position => new Vec3(this[0,3], this[1,3], this[2,3]);

    /// <summary>
    /// Number of rows in this matrix
    /// </summary>
    public int RowCount => 4;

    /// <summary>
    /// Number of columns in this matrix
    /// </summary>
    public int ColumnCount => 4;

    /// <summary>
    /// Create a new matrix with the given row/column elements
    /// </summary>
    /// <param name="e01"></param>
    /// <param name="e02"></param>
    /// <param name="e03"></param>
    /// <param name="e04"></param>
    /// <param name="e11"></param>
    /// <param name="e12"></param>
    /// <param name="e13"></param>
    /// <param name="e14"></param>
    /// <param name="e21"></param>
    /// <param name="e22"></param>
    /// <param name="e23"></param>
    /// <param name="e24"></param>
    private Transformation(
        double e01, double e02, double e03, double e04,
        double e11, double e12, double e13, double e14,
        double e21, double e22, double e23, double e24
    ) {
        this.rotation = new double[,] {
            {e01, e02, e03},
            {e11, e12, e13},
            {e21, e22, e23}
        };
        this.position = new double[]{
            e04,
            e14,
            e24
        };
    }

    /// <summary>
    /// Scaling transformation
    /// </summary>
    /// <param name="position"></param>
    /// <returns>scaling transformation</returns>
    public static Transformation Scale (Vec3 scale) {
       return new Transformation(
            scale.X, 0, 0, 0,
            0, scale.Y, 0, 0,
            0, 0, scale.Z, 0
        ); 
    }

    /// <summary>
    /// Movement transformation
    /// </summary>
    /// <param name="position"></param>
    /// <returns>displacement transformation</returns>
    public static Transformation Offset (Vec3 position) {
       return new Transformation(
            1, 0, 0, position.X,
            0, 1, 0, position.Y,
            0, 0, 1, position.Z
        ); 
    }

    /// <summary>
    /// Rotation from Euler angles Rz(gamma)Rx(theta)Rz(phi)
    /// </summary>
    /// <param name="gamma">first Z angle in radians</param>
    /// <param name="theta">X angles in radians</param>
    /// <param name="phi">last Z angle in radians</param>
    /// <returns>rotation transformation</returns>
    public static Transformation EulerRotation (double gamma, double theta, double phi) {
        var cg = System.Math.Cos(gamma);
        var ct = System.Math.Cos(theta);
        var cp = System.Math.Cos(phi);

        var sg = System.Math.Sin(gamma);
        var st = System.Math.Sin(theta);
        var sp = System.Math.Sin(phi);

        return new Transformation(
            cg*cp - sg*ct*sp,   -cg*sp - sg*ct*cp,  st*sg,  0,
            sg*cp + cg*ct*sp,   -sg*sp + cg*ct*cp,  -cg*st, 0,
            st*sp,              st*cp,              ct,     0
        );
    }

    /// <summary>
    /// Rotation around the x-axis
    /// </summary>
    /// <param name="angle">rotation angle in radians</param>
    /// <returns>rotation transformation</returns>
    public static Transformation Rx(double angle){
        var ca = Math.Cos(angle);
        var sa = Math.Sin(angle);
        return new Transformation(
            1,  0,  0,    0,
            0,  ca, -sa,  0,
            0,  sa, ca,   0
        );
    }

    /// <summary>
    /// Rotation around the y-axis
    /// </summary>
    /// <param name="angle">rotation angle in radians</param>
    /// <returns>rotation transformation</returns>
    public static Transformation Ry(double angle){
        var ca = Math.Cos(angle);
        var sa = Math.Sin(angle);
        return new Transformation(
            ca,  0,  sa,  0,
            0,   1,  0,   0,
            -sa, 0,  ca,  0
        );
    }

    /// <summary>
    /// Rotation around the z-axis
    /// </summary>
    /// <param name="angle">rotation angle in radians</param>
    /// <returns>rotation transformation</returns>
    public static Transformation Rz(double angle){
        var ca = Math.Cos(angle);
        var sa = Math.Sin(angle);
        return new Transformation(
            ca, -sa,  0,  0,
            sa,  ca,  0,  0,
            0,   0,   1,  0
        );
    }

    /// <summary>
    /// Multiply two transformations to create a new transformation
    /// </summary>
    /// <param name="lhs">first transformation</param>
    /// <param name="rhs">second transformation</param>
    /// <returns>combined transformation</returns>
    public static Transformation operator * (Transformation lhs, Transformation rhs) {
        // https://www.wolframalpha.com/input/?i=%7B%7Ba%2C+b%2C+c%2C+d%7D%2C+%7Be%2C+f%2C+g%2C+h%7D%2C+%7Bi%2C+j%2C+k%2C+l%7D%2C+%7B0%2C+0%2C+0%2C+z%7D%7D+*+%7B%7Bm%2C+n%2C+o%2C+p%7D%2C+%7Bq%2C+r%2C+s%2C+t%7D%2C+%7Bu%2C+v%2C+w%2C+x%7D%2C+%7B0%2C+0%2C+0%2C+y%7D%7D
        double a = lhs[0, 0];
        double b = lhs[0, 1];
        double c = lhs[0, 2];
        double d = lhs[0, 3];

        double e = lhs[1, 0];
        double f = lhs[1, 1];
        double g = lhs[1, 2];
        double h = lhs[1, 3];

        double i = lhs[2, 0];
        double j = lhs[2, 1];
        double k = lhs[2, 2];
        double l = lhs[2, 3];

        //double z = 1; // lhs[3,3]

        double m = rhs[0, 0];
        double n = rhs[0, 1];
        double o = rhs[0, 2];
        double p = rhs[0, 3];

        double q = rhs[1, 0];
        double r = rhs[1, 1];
        double s = rhs[1, 2];
        double t = rhs[1, 3];

        double u = rhs[2, 0];
        double v = rhs[2, 1];
        double w = rhs[2, 2];
        double x = rhs[2, 3];

        double y = rhs[3,3]; // == 1

        return new Transformation(
            a * m + b * q + c * u,  a * n + b * r + c * v,  a * o + b * s + c * w,  a * p + b * t + c * x + d * y,
            e * m + f * q + g * u,  e * n + f * r + g * v,  e * o + f * s + g * w,  e * p + f * t + g * x + h * y,
            i * m + j * q + k * u,  i * n + j * r + k * v,  i * o + j * s + k * w,  i * p + j * t + k * x + l * y
        );
    }

    /// <summary>
    /// Apply transformation to the given vector
    /// </summary>
    /// <param name="mtx">transformation</param>
    /// <param name="vec">vector</param>
    /// <returns>transformed vector</returns>
    public static Vec3 operator * (Transformation mtx, Vec3 vec) {
        // https://www.wolframalpha.com/input/?i=%7B%7Ba%2C+b%2C+c%2C+d%7D%2C+%7Be%2C+f%2C+g%2C+h%7D%2C+%7Bi%2C+j%2C+k%2C+l%7D%2C+%7B0%2C+0%2C+0%2C+s%7D%7D+*+%7B%7Bx%7D%2C+%7By%7D%2C+%7Bz%7D%2C+%7B1%7D%7D
        double a = mtx[0, 0];
        double b = mtx[0, 1];
        double c = mtx[0, 2];
        double d = mtx[0, 3];

        double e = mtx[1, 0];
        double f = mtx[1, 1];
        double g = mtx[1, 2];
        double h = mtx[1, 3];

        double i = mtx[2, 0];
        double j = mtx[2, 1];
        double k = mtx[2, 2];
        double l = mtx[2, 3];

        // double s = mtx[3, 3]; == 1

        double Px = d + a * vec.X + b * vec.Y + c * vec.Z;
        double Py = h + e * vec.X + f * vec.Y + g * vec.Z;
        double Pz = l + i * vec.X + j * vec.Y + k * vec.Z;

        return new Vec3(
            Px,
            Py,
            Pz
        );
    }

    /// <summary>
    /// Print matrix to string
    /// </summary>
    public override string ToString() {
        return $"[{this[0,0]},{this[0,1]},{this[0,2]},{this[0,3]};{this[1,0]},{this[1,1]},{this[1,2]},{this[1,3]};{this[2,0]},{this[2,1]},{this[2,2]},{this[2,3]};{this[3,0]},{this[3,1]},{this[3,2]},{this[3,3]}]";
    }

}

}