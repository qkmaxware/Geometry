using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Orthogonal set of basis vectors as described by a transformation from the world origin
/// </summary>
public class Basis {

    /// <summary>
    /// Transformation describing this basis
    /// </summary>
    /// <value>transformation</value>
    public Transformation Transform {get; set;}

    /// <summary>
    /// Create a new Basis at the world origin
    /// </summary>
    public Basis () {
        this.Transform = Transformation.Identity();
    }

    /// <summary>
    /// Create a new basis described by a transformation from the world origin
    /// </summary>
    /// <param name="transformation">basis transformation</param>
    public Basis (Transformation transformation) {
        this.Transform = transformation;
    }

    /// <summary>
    /// World X axis of this reference frame
    /// </summary>
    public Vec3 X => new Vec3(Transform[0,0], Transform[1,0], Transform[2,0]);
    /// <summary>
    /// World Y axis of this reference frame
    /// </summary>
    public Vec3 Y => new Vec3(Transform[0,1], Transform[1,1], Transform[2,1]);
    /// <summary>
    /// World Z axis of this reference frame
    /// </summary>
    public Vec3 Z => new Vec3(Transform[0,2], Transform[1,2], Transform[2,2]);
    /// <summary>
    /// World position of this reference frame
    /// </summary>
    public Vec3 Position {
        get => new Vec3(Transform[0,3], Transform[1,3], Transform[2,3]);
        set {
            this.Transform = new Transformation(
                this.Transform[0,0], this.Transform[0,1], this.Transform[0,2], value.X,
                this.Transform[1,0], this.Transform[1,1], this.Transform[1,2], value.Y,
                this.Transform[2,0], this.Transform[2,1], this.Transform[2,2], value.Z
            );
        }
    }

    /// <summary>
    /// World rotation angles about the X,Y,Z axis
    /// </summary>
    public Vec3 Rotation {
        get {
            var sy = Math.Sqrt(
                Transform[0,0] * Transform[0,0] + Transform[1,0] * Transform[1,0]
            );
            var singular = sy < Double.Epsilon;
            double x,y,z;
            if (!singular) {
                x = Math.Atan2(Transform[2,1], Transform[2,2]);
                y = Math.Atan2(-Transform[2,0], sy);
                z = Math.Atan2(Transform[1,0], Transform[0,0]);
            } else {
                x = Math.Atan2(-Transform[1,2], Transform[1,1]);
                y = Math.Atan2(-Transform[2,0], sy);
                z = 0;
            }
            return new Vec3(x,y,z);
        }
    }

    /// <summary>
    /// Rotate this basis by the given quaterion
    /// </summary>
    /// <param name="rotation">rotation</param>
    public void Rotate (Quat rotation) {
        this.Transform = rotation * this.Transform;
    }

    /// <summary>
    /// Rotate this basis around a point in space
    /// </summary>
    /// <param name="point">point to orbit</param>
    /// <param name="axis">axis to orbit around</param>
    /// <param name="angle">angle to rotate</param>
    public void RotateAround(Vec3 point, Vec3 axis, double angle) {
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

        this.Transform = (Transformation.Offset(point) * rotation * Transformation.Offset(-point)) * this.Transform;
    }

    /// <summary>
    /// Look at a point in world space
    /// </summary>
    /// <param name="position">world space position</param>
    public void LookAt(Vec3 position) {
        this.Rotate(Quat.FromToRotation(this.Y, position - this.Position));
    }

    /// <summary>
    /// Move this basis by a given amount in world space 
    /// </summary>
    /// <param name="delta">amount to move</param>
    public void TranslateAbsolute (Vec3 delta) {
        this.Transform = Transformation.Offset(delta) * this.Transform;
    }

    /// <summary>
    /// Move this basis by a given amount in local space
    /// </summary>
    /// <param name="delta">amount to move</param>
    public void TranslateRelative (Vec3 delta) {
        this.Transform =  this.Transform * Transformation.Offset(delta);
    }

    /// <summary>
    /// Transform a point from local space to world space
    /// </summary>
    /// <param name="local">local point</param>
    /// <returns>world point</returns>
    public Vec3 TransformToWorldSpace (Vec3 local) {
        return this.Transform * local;
    }

    /// <summary>
    /// Transform a point from world space to local
    /// </summary>
    /// <param name="global"></param>
    /// <returns></returns>
    public Vec3 TransformToLocalSpace (Vec3 global) {
        return this.Transform.Inverse * global;
    }

    /// <summary>
    /// Transform a point from local space in another basis to local space in this basis
    /// </summary>
    /// <param name="other">other basis</param>
    /// <param name="local">point in other basis</param>
    /// <returns>point in local space</returns>
    public Vec3 TransformFromBasis(Basis other, Vec3 local) {
        var global = other.TransformToWorldSpace(local);
        return this.TransformToLocalSpace(global);
    }
}

}