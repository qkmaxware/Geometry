using System;

namespace Qkmaxware.Geometry.Coordinates {

/// <summary>
/// Basis vector set
/// </summary>
public class Basis {
    /// <summary>
    /// Local 'X' axis
    /// </summary>
    /// <value>Local 'X' axis</value>
    public Vec3 X {get; private set;}
    /// <summary>
    /// Local 'Y' axis
    /// </summary>
    /// <value>Local 'Y' axis</value>
    public Vec3 Y {get; private set;}
    /// <summary>
    /// Local 'Z' axis
    /// </summary>
    /// <value>Local 'Z' axis</value>
    public Vec3 Z {get; private set;}

    /// <summary>
    /// Create a new basis vector set with the standard X,Y,Z axis
    /// </summary>
    public Basis() {
        this.X = Vec3.I;
        this.Y = Vec3.J;
        this.Z = Vec3.K;
    }

    /// <summary>
    /// Create a new basis vector set with the given axis, vectors should be orthogonal
    /// </summary>
    /// <param name="xb">local x axis</param>
    /// <param name="yb">local y axis</param>
    /// <param name="zb">local z axis</param>
    public Basis(Vec3 x, Vec3 y, Vec3 z) {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}

/// <summary>
/// Base class for a frame of reference
/// </summary>
public class Frame {
    /// <summary>
    /// Basis vectors for this frame of reference
    /// </summary>
    /// <returns>basis vector set</returns>
    public Basis Basis => new Basis(this.LocalToGlobalDirection(Vec3.I), this.LocalToGlobalDirection(Vec3.J), this.LocalToGlobalDirection(Vec3.K));

    /// <summary>
    /// The rotation of the frame relative to its parent
    /// </summary>
    /// <value>The rotation of the frame relative to its parent</value>
    public Quat LocalRotation {get; set;}

    /// <summary>
    /// The position of the frame relative to its parent
    /// </summary>
    /// <value>The position of the frame relative to its parent</value>
    public Vec3 LocalPosition {get; set;}

    /// <summary>
    /// Parent frame this one is attached to
    /// </summary>
    /// <value>parent frame if one exists</value>
    public Frame? Parent {get; set;}

    /// <summary>
    /// Create a new frame of reference at the world origin
    /// </summary>
    public Frame () {
        this.LocalRotation = Quat.Identity;
        this.LocalPosition = Vec3.Zero;
    }

    /// <summary>
    /// Create a new frame of reference at the world origin with the given orientation
    /// </summary>
    /// <param name="rotation">orientation</param>
    public Frame (Quat rotation) {
        this.LocalRotation = rotation;
        this.LocalPosition = Vec3.Zero;
    }

    /// <summary>
    /// Create a new frame of reference at the given position with the given orientation
    /// </summary>
    /// <param name="rotation">orientation</param>
    /// <param name="pos">position</param>
    public Frame (Quat rotation, Vec3 pos) {
        this.LocalRotation = rotation;
        this.LocalPosition = pos;
    }

    private Transformation createLocalToParentMatrix () {
        return Transformation.OffsetRotation(this.LocalRotation, this.LocalPosition);
    }
    private Quat createLocalToParentRotation() {
        return this.LocalRotation;
    }
    private Transformation createParentToLocalMatrix() {
        return createParentToLocalMatrix().Inverse;
    }
    private Quat createParentToLocalRotation() {
        return this.LocalRotation.Conjugate;
    }

    private Transformation createLocalToGlobalMatrix() {
        var my_matrix = createLocalToParentMatrix();
        if (Parent != null) {
            return Parent.createLocalToGlobalMatrix() * my_matrix;
        } else {
            return my_matrix;
        }
    }
    private Quat createLocalToGlobalRotation() {
        var my_rot = createLocalToParentRotation();
        if (Parent != null) {
            return Parent.createLocalToGlobalRotation() * my_rot;
        } else {
            return my_rot;
        }
    }
    private Transformation createGlobalToLocalMatrix() {
        return createLocalToGlobalMatrix().Inverse;
    }
    private Quat createGlobalToLocalRotation() {
        return createLocalToGlobalRotation().Conjugate;
    }

    /// <summary>
    /// Convert a point from local space to a point in the parent space
    /// </summary>
    /// <param name="point">local space point</param>
    /// <returns>point in parent space</returns>
    public Vec3 LocalToParentPoint(Vec3 point) {
        var mtx = createLocalToParentMatrix();
        return mtx * point;
    }

    /// <summary>
    /// Convert a point from parent space to a point in local space
    /// </summary>
    /// <param name="point">parent space point</param>
    /// <returns>point in local space</returns>
    public Vec3 ParentToLocalPoint(Vec3 point) {
        var mtx = createParentToLocalMatrix();
        return mtx * point;
    }

    /// <summary>
    /// Convert a point from local space to global space
    /// </summary>
    /// <param name="point">local space point</param>
    /// <returns>point in global space</returns>
    public Vec3 LocalToGlobalPoint(Vec3 point) {
        var mtx = createLocalToGlobalMatrix();
        return mtx * point;
    }

    /// <summary>
    /// Rotate a direction from local space to global space without moving it
    /// </summary>
    /// <param name="direction">direction</param>
    /// <returns>rotated direction</returns>
    public Vec3 LocalToGlobalDirection(Vec3 direction) {
        return ((Transformation)createLocalToGlobalRotation()) * direction;
    }

    /// <summary>
    /// Convert a point from global space to local space
    /// </summary>
    /// <param name="point">global space point</param>
    /// <returns>point in local space</returns>
    public Vec3 GlobalToLocalPoint(Vec3 point) {
        var mtx = createGlobalToLocalMatrix();
        return mtx * point;
    }

    /// <summary>
    /// Rotate a direction from global space to local space without moving it
    /// </summary>
    /// <param name="direction">direction</param>
    /// <returns>rotated direction</returns>
    public Vec3 GlobalToLocalDirection(Vec3 direction) {
        return ((Transformation)createGlobalToLocalRotation()) * direction;
    }

    /// <summary>
    /// Convert a point from local space in this frame of reference to the space of another frame of reference
    /// </summary>
    /// <param name="point">local space point</param>
    /// <param name="frame">frame to convert to</param>
    /// <returns>point in the other frame</returns>
    public Vec3 LocalToFramePoint(Vec3 point, Frame frame) {
        return frame.GlobalToLocalPoint(this.LocalToGlobalPoint(point));
    }

    /// <summary>
    /// Convert a point from the local space of another frame of reference to a local point in this frame of reference
    /// </summary>
    /// <param name="frame">frame to convert from</param>
    /// <param name="point">point in the frame</param>
    /// <returns>point in local space</returns>
    public Vec3 FrameToLocalPoint(Frame frame, Vec3 point) {
        return this.GlobalToLocalPoint(frame.LocalToGlobalPoint(point));
    }
}
    
}