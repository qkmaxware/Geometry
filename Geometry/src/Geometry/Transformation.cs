using System;
using System.Linq;
using Qkmaxware.Geometry;

namespace Qkmaxware.Geometry{

/// <summary>
/// Affine transformation
/// </summary>
public class Transformation {
    private double[,] rotation; // 3x3 matrix
    private double[] position; // 1x3 vector position

    /// <summary>
    /// Index element from this transformation matrix
    /// </summary>
    public double this[int row, int col] {
        get {
            if (row < 3 && col < 3) {
                return rotation[row, col]; 
            } else if (row < 3 && col == 3) {
                return position[row];
            } else if (row == 3 && col == 3) {
                return 1;
            } else {
                return 0;
            }
        } set {
            if (row < 3 && col < 3) {
                rotation[row, col] = value; 
            } else if (row < 3 && col == 3) {
                position[row] = value;
            } 
        }
    }

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
    public Transformation(
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
    /// Compute the inverse of this transformation
    /// </summary>
    /// <value>inverse transformation matrix</value>
    public Transformation Inverse {
        get {
            var s0 = this[0, 0] * this[1, 1] - this[1, 0] * this[0, 1];
            var s1 = this[0, 0] * this[1, 2] - this[1, 0] * this[0, 2];
            var s2 = this[0, 0] * this[1, 3] - this[1, 0] * this[0, 3];
            var s3 = this[0, 1] * this[1, 2] - this[1, 1] * this[0, 2];
            var s4 = this[0, 1] * this[1, 3] - this[1, 1] * this[0, 3];
            var s5 = this[0, 2] * this[1, 3] - this[1, 2] * this[0, 3];

            var c5 = this[2, 2] * this[3, 3] - this[3, 2] * this[2, 3];
            var c4 = this[2, 1] * this[3, 3] - this[3, 1] * this[2, 3];
            var c3 = this[2, 1] * this[3, 2] - this[3, 1] * this[2, 2];
            var c2 = this[2, 0] * this[3, 3] - this[3, 0] * this[2, 3];
            var c1 = this[2, 0] * this[3, 2] - this[3, 0] * this[2, 2];
            var c0 = this[2, 0] * this[3, 1] - this[3, 0] * this[2, 1];

            var det = (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);
            if (det == 0) {
                throw new DivideByZeroException();
            }
            var invdet = 1.0 / det;

            return new Transformation(
                ( this[1, 1] * c5 - this[1, 2] * c4 + this[1, 3] * c3) * invdet,
                (-this[0, 1] * c5 + this[0, 2] * c4 - this[0, 3] * c3) * invdet,
                ( this[3, 1] * s5 - this[3, 2] * s4 + this[3, 3] * s3) * invdet,
                (-this[2, 1] * s5 + this[2, 2] * s4 - this[2, 3] * s3) * invdet,

                (-this[1, 0] * c5 + this[1, 2] * c2 - this[1, 3] * c1) * invdet,
                ( this[0, 0] * c5 - this[0, 2] * c2 + this[0, 3] * c1) * invdet,
                (-this[3, 0] * s5 + this[3, 2] * s2 - this[3, 3] * s1) * invdet,
                ( this[2, 0] * s5 - this[2, 2] * s2 + this[2, 3] * s1) * invdet,

                ( this[1, 0] * c4 - this[1, 1] * c2 + this[1, 3] * c0) * invdet,
                (-this[0, 0] * c4 + this[0, 1] * c2 - this[0, 3] * c0) * invdet,
                ( this[3, 0] * s4 - this[3, 1] * s2 + this[3, 3] * s0) * invdet,
                (-this[2, 0] * s4 + this[2, 1] * s2 - this[2, 3] * s0) * invdet

                //(-this[1, 0] * c3 + this[1, 1] * c1 - this[1, 2] * c0) * invdet,
                //( this[0, 0] * c3 - this[0, 1] * c1 + this[0, 2] * c0) * invdet,
                //(-this[3, 0] * s3 + this[3, 1] * s1 - this[3, 2] * s0) * invdet,
                //( this[2, 0] * s3 - this[2, 1] * s1 + this[2, 2] * s0) * invdet
            );
        }
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
    /// Rotation about arbitrary axis
    /// </summary>
    /// <param name="axis">axis of rotation</param>
    /// <param name="angle">rotational angle</param>
    /// <returns>rotation transformation</returns>
    public static Transformation AngleAxis(Vec3 axis, double angle) {
        var norm = axis.Normalized;
        double c = Math.Cos(angle);
        double s = Math.Sin(angle);
        double t = 1.0 - c;
        double x = norm.X;
        double y = norm.Y;
        double z = norm.Z;

        var rotation = new Transformation(
            t * x * x + c,          t * x * y - z * s,      t * x * z + y * s,      0,
            t * x * y + z * s,      t * y * y + c,          t * y * z - x * s,      0,
            t * x * z - y * s,      t * y * z + x * s,      t * z * z + c,          0
        );
        return rotation;
    }

    /// <summary>
    /// Create a transformation from a given rotation and offset
    /// </summary>
    /// <param name="rotation">rotation quaternion</param>
    /// <param name="offset">offset position</param>
    /// <returns>transformation</returns>
    public static Transformation OffsetRotation(Quat rotation, Vec3 offset) {
        var rot_matrix = (Transformation)rotation;
        return new Transformation(
            rot_matrix[0,0], rot_matrix[0,1], rot_matrix[0,2], offset.X,
            rot_matrix[1,0], rot_matrix[1,1], rot_matrix[1,2], offset.Y,
            rot_matrix[2,0], rot_matrix[2,1], rot_matrix[2,2], offset.Z
        );
    }

    /// <summary>
    /// Identity matrix
    /// </summary>
    /// <returns>identity transformation</returns>
    public static Transformation Identity() {
        return new Transformation(
            1,  0,  0,  0,
            0,  1,  0,  0,
            0,  0,  1,  0
            //          1
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
    /// Apply a transformation to a mesh's triangles
    /// </summary>
    /// <param name="matrix">transformation matrix</param>
    /// <param name="mesh">original mesh</param>
    /// <returns>new transformed mesh</returns>
    public static IMesh operator * (Transformation matrix, IMesh mesh) {
        return new ListMesh(mesh.Select(tri => tri.Transform(matrix)));
    }

    /// <summary>
    /// Print matrix to string
    /// </summary>
    public override string ToString() {
        return $"[{this[0,0]},{this[0,1]},{this[0,2]},{this[0,3]};{this[1,0]},{this[1,1]},{this[1,2]},{this[1,3]};{this[2,0]},{this[2,1]},{this[2,2]},{this[2,3]};{this[3,0]},{this[3,1]},{this[3,2]},{this[3,3]}]";
    }

}

}