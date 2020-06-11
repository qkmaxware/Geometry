using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// 3d Axis aligned bounding box
/// </summary>
public class Box3 {
    /// <summary>
    /// Centre of the box
    /// </summary>
    public Vec3 Centre => (Max + Min) * 0.5;
    /// <summary>
    /// Extents of the box, these are always one half of the total size
    /// </summary>
    public Vec3 Extents => Size * 0.5;
    /// <summary>
    /// Total length of each of the box's sides
    /// </summary>
    public Vec3 Size => Max - Min;
    /// <summary>
    /// Min corner of the box
    /// </summary>
    public Vec3 Max {get; private set;}
    /// <summary>
    /// Max corner of the box
    /// </summary>
    public Vec3 Min {get; private set;}

    /// <summary>
    /// Create a new box
    /// </summary>
    /// <param name="min">min coordinate</param>
    /// <param name="max">max coordinate</param>
    public Box3 (Vec3 min, Vec3 max) {
        this.Min = Vec3.Min(min, max);
        this.Max = Vec3.Max(min, max);
    }

    /// <summary>
    /// Create a new bounding box that contains both of the source boxes
    /// </summary>
    /// <param name="a">first bounding box</param>
    /// <param name="b">second bounding box</param>
    /// <returns>bounding box that contains the two</returns>
    public static Box3 Merge(Box3 a, Box3 b) {
        return new Box3(Vec3.Min(a.Min, b.Min), Vec3.Max(a.Max, b.Max));
    }

    /// <summary>
    /// Check if a point is contained within this boc
    /// </summary>
    /// <param name="point">point to check</param>
    /// <returns>true if point is contained</returns>
    public bool Contains (Vec3 point) {
        return 
            (Min.X <= point.X && point.X <= Max.X)
         && (Min.Y <= point.Y && point.Y <= Max.Y)
         && (Min.Z <= point.Z && point.Z <= Max.Z);
    }

    /// <summary>
    /// Test if this box intersects another
    /// </summary>
    /// <param name="box">another box</param>
    /// <returns>true if the boxes intersect</returns>
    public bool Intersects (Box3 box) {
        return 
            (this.Min.X <= box.Max.X && this.Max.X >= box.Min.X)
         && (this.Min.Y <= box.Max.Y && this.Max.Y >= box.Min.Y)
         && (this.Min.Z <= box.Max.Z && this.Max.Z >= box.Min.Z);
    }

    /// <summary>
    /// Check if a ray intersects with this box
    /// </summary>
    /// <param name="ray">ray to check</param>
    /// <returns>true if the ray intersects this box</returns>
    public bool Intersects (Ray ray) {
        Vec3 pos;
        if (ray.Cast(this, out pos)) {
            return true;
        } else {
            return false;
        }
    }

    private static bool TestAxis(Vec3 v0, Vec3 v1, Vec3 v2, Vec3 e, Vec3 u0, Vec3 u1, Vec3 u2, Vec3 axis) {
        double p0 = Vec3.Dot(v0, axis);
        double p1 = Vec3.Dot(v1, axis);
        double p2 = Vec3.Dot(v2, axis);
        double r = e.X * Math.Abs(Vec3.Dot(u0, axis)) +
                e.Y * Math.Abs(Vec3.Dot(u1, axis)) +
                e.Z * Math.Abs(Vec3.Dot(u2, axis));
        if (Math.Max(-Math.Max(p0, Math.Max(p1, p2)), Math.Min(p0,Math.Min(p1, p2))) > r) {
            return false;
        } else{
            return true;
        }
    }

    /// <summary>
    /// Test if triangle intersects with this bounding volume
    /// </summary>
    /// <param name="other">triangle</param>
    /// <returns>true if triangle is contained or intersecting with the volume</returns>
    public bool Intersects(Triangle other) {
        Vec3 v0 = other.Item1;
        Vec3 v1 = other.Item2;
        Vec3 v2 = other.Item3;

        // Convert AABB to center-extents form
        Vec3 c = this.Centre;
        Vec3 e = this.Extents;

        // Translate the triangle as conceptually moving the AABB to origin
        v0 -= c;
        v1 -= c;
        v2 -= c;

        // Compute the edge vectors of the triangle  (ABC)
        Vec3 f0 = v1 - v0; // B - A
        Vec3 f1 = v2 - v1; // C - B
        Vec3 f2 = v0 - v2; // A - C

        // Compute the face normals of the AABB, because the AABB
        Vec3 u0 = new Vec3(1.0f, 0.0f, 0.0f);
        Vec3 u1 = new Vec3(0.0f, 1.0f, 0.0f);
        Vec3 u2 = new Vec3(0.0f, 0.0f, 1.0f);

        // There are a total of 13 axis to test!
        // Compute the 9 axis
        Vec3 axis_u0_f0 = Vec3.Cross(u0, f0);
        Vec3 axis_u0_f1 = Vec3.Cross(u0, f1);
        Vec3 axis_u0_f2 = Vec3.Cross(u0, f2);

        Vec3 axis_u1_f0 = Vec3.Cross(u1, f0);
        Vec3 axis_u1_f1 = Vec3.Cross(u1, f1);
        Vec3 axis_u1_f2 = Vec3.Cross(u2, f2);

        Vec3 axis_u2_f0 = Vec3.Cross(u2, f0);
        Vec3 axis_u2_f1 = Vec3.Cross(u2, f1);
        Vec3 axis_u2_f2 = Vec3.Cross(u2, f2);

        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u0_f0)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u0_f1)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u0_f2)) {
            return false;
        }

        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u1_f0)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u1_f1)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u1_f2)) {
            return false;
        }

        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u2_f0)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u2_f1)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, axis_u2_f2)) {
            return false;
        }

        // Next, we have 3 face normals from the AABB
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, u0)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, u1)) {
            return false;
        }
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, u2)) {
            return false;
        }

        // Finally, we have one last axis to test, the face normal of the triangle
        if(!TestAxis(v0, v1, v2, e, u0, u1, u2, other.Normal)) {
            return false;
        }

        // Passed testing for all 13 seperating axis that exist!
        return true;
    }

    public override string ToString() {
        return String.Format("(min:{0},max:{1})", this.Min, this.Max);
    }
}

}