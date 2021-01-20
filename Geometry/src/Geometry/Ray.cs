using System;
using System.Collections.Generic;
using System.Linq;

namespace Qkmaxware.Geometry {

/// <summary>
/// Represents a ray in 3d space
/// </summary>
public class Ray {
    /// <summary>
    /// Origin of the ray
    /// </summary>
    public Vec3 Origin {get; private set;}
    /// <summary>
    /// Direction of the ray
    /// </summary>
    public Vec3 Direction {get; private set;}

    /// <summary>
    /// Evaluate a point along this ray at the given distance
    /// </summary>
    public Vec3 this[double length] => this.Origin + this.Direction * length;

    /// <summary>
    /// Create a ray
    /// </summary>
    /// <param name="origin">origin of the ray</param>
    /// <param name="direction">direction of travel</param>
    public Ray (Vec3 origin, Vec3 direction) {
        this.Origin = origin;
        this.Direction = direction.Normalized;
    }

    /// <summary>
    /// Determine if this ray intersects with the given triangle with the Möller–Trumbore algorithm
    /// </summary>
    /// <param name="triangle">3d triangle</param>
    /// <returns>true if there was a collision</returns>
    public bool Cast(Triangle triangle) {
        Vec3 hit;
        return Cast(triangle, out hit);
    }

    /// <summary>
    /// Determine if this ray intersects with the given box3
    /// </summary>
    /// <param name="aabb">3d box</param>
    /// <param name="hit">the coordinate of the collision</param>
    /// <returns>true if there was a collision</returns>
    public bool Cast(Box3 aabb, out Vec3 hit) {
        double t1 = (aabb.Min.X - this.Origin.X) / this.Direction.X;
        double t2 = (aabb.Max.X - this.Origin.X) / this.Direction.X;
        double t3 = (aabb.Min.Y - this.Origin.Y) / this.Direction.Y;
        double t4 = (aabb.Max.Y - this.Origin.Y) / this.Direction.Y;
        double t5 = (aabb.Min.Z - this.Origin.Z) / this.Direction.Z;
        double t6 = (aabb.Max.Z - this.Origin.Z) / this.Direction.Z;

        double tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
        double tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

        // if tmax < 0, ray (line) is intersecting AABB, but whole AABB is behind us
        if (tmax < 0) {
            hit = Origin;
            return false;
        }

        // if tmin > tmax, ray doesn't intersect AABB
        if (tmin > tmax) {
            hit = Origin;
            return false;
        }
        
        if (tmin < 0f) {
            hit = this[tmax];
            return true; // tmax is the distance
        } else {
            hit = this[tmin];
            return true; // tmin is the distance
        }
    }

    /// <summary>
    /// Determine if this ray intersects with the given triangle with the Möller–Trumbore algorithm
    /// </summary>
    /// <param name="triangle">3d triangle</param>
    /// <param name="hit">the coordinate of the hit if it exists</param>
    /// <returns>true if there was a collision</returns>
    public bool Cast(Triangle triangle, out Vec3 hit) {
        hit = Vec3.Zero;

        // Find edges for the 2 vectors sharing point 0
        Vec3 edge1 = triangle.Item2 - triangle.Item1;
        Vec3 edge2 = triangle.Item3 - triangle.Item1;

        Vec3 pvec,tvec,qvec;
        double det,invDet,u,v;

        // Check if the ray is parallel to the triangle and can't intersect
        pvec = Vec3.Cross(this.Direction, edge2);
        det = Vec3.Dot(edge1, pvec);
        if (det > -double.Epsilon && det < double.Epsilon) {
            return false;
        }

        invDet = 1.0/det;
        tvec = this.Origin - triangle.Item1;
        u = invDet * (Vec3.Dot(tvec, pvec));
        if (u < 0.0 || u > 1.0) {
            return false;
        }

        qvec = Vec3.Cross(tvec, edge1);
        v = invDet * Vec3.Dot(this.Direction, qvec);
        if (v < 0.0 || (u + v) > 1.0) {
            return false;
        }

        // Find t to find out where the intersection point is on the line.
        double t = invDet * Vec3.Dot(edge2, qvec);
        if (t > double.Epsilon) {
            hit = this.Origin + t * this.Direction;
            return true;
        } else {
            return false;
        }
    }

    /// <summary>
    /// Determine if this ray intersects the given solid
    /// </summary>
    /// <param name="solid">solid to intersect</param>
    /// <returns>true if the ray collides</returns>
    public bool Cast(IMesh solid) {
        Vec3 hit;
        return Cast(solid, out hit);
    }

    /// <summary>
    /// Determine if this ray intersects the given solid
    /// </summary>
    /// <param name="solid">solid to intersect</param>
    /// <param name="hit">the first collision point</param>
    /// <returns>true if the ray collides</returns>
    public bool Cast(IMesh solid, out Vec3 hit) {
        hit = Vec3.Zero;
        foreach(Triangle tri in solid) {
            if(Cast(tri, out hit)) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determine if this ray intersects the given solid
    /// </summary>
    /// <param name="solid">the collection of triangles to check</param>
    /// <returns>true if there were any collisions</returns>
    public List<Vec3> CastAll(IMesh solid) {
        HashSet<Vec3> found = new HashSet<Vec3>();
        Vec3 hit;
        foreach(Triangle tri in solid) {
            if(Cast(tri, out hit)) {
                if (!found.Contains(hit)) { // Remove duplicate hits
                    found.Add(hit);
                }
            }
        }
        return found.ToList();
    }

    /// <summary>
    /// Closest point on this ray to the given point
    /// </summary>
    /// <param name="position">point</param>
    /// <returns>closest point</returns>
    public Vec3 ClosestPointTo(Vec3 position) {
        Vec3 a = this.Origin;
        Vec3 b = this.Origin + this.Direction;

        // Project position onto ab
        double t = Vec3.Dot(position - a, this.Direction) / Vec3.Dot(this.Direction, this.Direction);

        // Compute the point
        return a + Math.Max(t , 0) * this.Direction;
    }

    /// <summary>
    /// Convert ray to string
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return string.Format("(origin:{0},direction:{1})", this.Origin, this.Direction);
    } 
}

}