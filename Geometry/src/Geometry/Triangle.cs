using System;

namespace Qkmaxware.Geometry {

/// <summary>
/// Triangles are composed of 3 vertices lying on a plane in 3 space
/// </summary>
public class Triangle : System.Tuple<Vec3, Vec3, Vec3> {

     /// <summary>
     /// Plane in which the vertices lie
     /// </summary>
     /// <value>vertex plane</value>
     public Plane Plane {get; private set;}

     /// <summary>
     /// Edge between vertex 1 and 2
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge12 {
          get {
               return Item2 - Item1;
          }
     }

     /// <summary>
     /// Edge between vertex 2 and 1
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge21 {
          get {
               return Item1 - Item2;
          }
     }

     /// <summary>
     /// Edge between vertex 1 and 3
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge13 {
          get {
               return Item3 - Item1;
          }
     }

     /// <summary>
     /// Edge between vertex 3 and 1
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge31 {
          get {
               return Item1 - Item3;
          }
     }

     /// <summary>
     /// Edge between vertex 2 and 3
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge23 {
          get {
               return Item3 - Item2;
          }
     }

     /// <summary>
     /// Edge between vertex 3 and 2
     /// </summary>
     /// <value>vector</value>
     public Vec3 Edge32 {
          get {
               return Item2 - Item3;
          }
     }

     /// <summary>
     /// Barycenter (centroid) of the triangle
     /// </summary>
     /// <value>average of all triangle vertices</value>
     public Vec3 Centre {
          get {
               return (Item1 + Item2 + Item3) / 3;
          }
     }

     /// <summary>
     /// Face normal
     /// </summary>
     /// <value>normal of the plane in which the vertices lie</value>
     public Vec3 Normal {
          get {
               return Plane.Normal;
          }
     }

     /// <summary>
     /// Creates a new triangle with a flipped normal by rotating the vertex winding order
     /// </summary>
     /// <value>flipped triangle</value>
     public Triangle Flipped {
          get {
               return new Triangle(this.Item2, this.Item3, this.Item1);
          }
     }

     private Box3? box = null;
     /// <summary>
     /// Compute an axis aligned bounding box that completely contains the triangle
     /// </summary>
     /// <value></value>
     public Box3 Bounds {
          get {
            if (box != null)
                return box;
            else {
                var mid  = this.Centre;
                var rad1 = (Item1 - mid).Length;
                var rad2 = (Item2 - mid).Length;
                var rad3 = (Item3 - mid).Length;
                var length = Math.Max(rad1, Math.Max(rad2, rad3));

                Vec3 offset = new Vec3(length, length, length);
                this.box = new Box3(mid - offset, mid + offset);
                return (Box3)box;   
            }
          }
     }

     /// <summary>
     /// Create a triangle from the given corners
     /// </summary>
     /// <param name="a">corner 1</param>
     /// <param name="b">corner 2</param>
     /// <param name="c">corner 3</param>
     /// <returns></returns>
     public Triangle(Vec3 a, Vec3 b, Vec3 c) : base(a,b,c) {
          this.Plane = new Plane(a,b,c);
     }

     private bool sameSide(Vec3 p1, Vec3 p2, Vec3 a, Vec3 b) {
          Vec3 cp1 = Vec3.Cross(b - a, p1 - a);
          Vec3 cp2 = Vec3.Cross(b - a, p2 - a);
          if (Vec3.Dot(cp1, cp2) >= 0) {
               return true;
          } else {
               return false;
          }
     }

     /// <summary>
     /// Does this triangle contain the given point
     /// </summary>
     /// <param name="point">point</param>
     /// <returns>true if triangle contains the given point</returns>
     public bool Contains(Vec3 point) {
          if (
               sameSide(point, Item1, Item2, Item3) &&
               sameSide(point, Item2, Item1, Item3) && 
               sameSide(point, Item3, Item1,Item2)
          ){ 
             return true;
        } else {
             return false;
        }
     }

     /// <summary>
     /// Closest point on the triangle to another position in space
     /// </summary>
     /// <param name="position">position</param>
     /// <returns>closest point on triangle to the given position</returns>
     public Vec3 ClosestPointTo(Vec3 position) {
          Vec3 point = this.Plane.ClosestPointTo(position);

          if (this.Contains(point)) {
               return point;
          }

          Line3 AB = new Line3(Item1, Item2);
          Line3 BC = new Line3(Item2, Item3);
          Line3 CA = new Line3(Item3, Item1);

          Vec3 c1 = AB.ClosestPointTo(position);
          Vec3 c2 = BC.ClosestPointTo(position);
          Vec3 c3 = CA.ClosestPointTo(position);

          double mag1 = (position - c1).SqrLength;
          double mag2 = (position - c2).SqrLength;
          double mag3 = (position - c3).SqrLength;

          double min = Math.Min(mag1, Math.Min(mag2, mag3));

          if (min == mag1) {
               return c1;
          } else if (min == mag2) {
               return c2;
          } else {
               return c3;
          }
     }

     /// <summary>
     /// Compute the intersection line between two triangles
     /// </summary>
     /// <param name="other">triangle to intersect with</param>
     /// <param name="line">line of intersection</param>
     /// <returns>true if the triangles intersect</returns>
     public bool Intersection (Triangle other, out Line3 line) {
          line = new Line3(Vec3.Zero, Vec3.Zero);

          int index = 0;
          Vec3[] points = new Vec3[2];

          Ray this12 = new Ray(Item1, Edge12);
          Ray this13 = new Ray(Item1, Edge13);
          Ray this23 = new Ray(Item2, Edge23);

          Ray other12 = new Ray(other.Item1, other.Edge12);
          Ray other13 = new Ray(other.Item1, other.Edge13);
          Ray other23 = new Ray(other.Item2, other.Edge23);

          // Check all 3 edges of triangle 1
          Vec3 hit;
          if (index < 2 && this12.Cast(other, out hit)) {
               points[index++] = hit;
          }
          if (index < 2 && this13.Cast(other, out hit)) {
               points[index++] = hit;
          }
          if (index < 2 && this23.Cast(other, out hit)) {
               points[index++] = hit;
          }

          // Check all 3 edges of triangle 2 
          if (index < 2 && other12.Cast(this, out hit)) {
               if (index == 0 || (!points[index-1].Equals(hit))) {
                    points[index++] = hit;
               }  
          }
          if (index < 2 && other13.Cast(this, out hit)) {
               if (index == 0 || (!points[index-1].Equals(hit))) {
                    points[index++] = hit;
               }  
          }
          if (index < 2 && other23.Cast(this, out hit)) {
               if (index == 0 || (!points[index-1].Equals(hit))) {
                    points[index++] = hit;
               }  
          }

          if (index < 2) {
               // Didn't intersect
               return false;
          } else {
               // Did intersect, return line
               line = new Line3(points[0], points[1]);
               return true;
          }
     }

    private static bool Between (double val, double start, double end) {
        if (start > end) {
            return val <= start && val >= end;
        } else {
            return val >= start && val <= end;
        }
    }

     /// <summary>
     /// Check if two triangles intersect
     /// </summary>
     /// <param name="other">triangle to intersect with</param>
     /// <returns>true if triangles intersect</returns>
     public bool Intersects(Triangle other) {
          // Compute the plane equation of triangle 2 
 
          // Reject as trivial if all points of triangle 1 are on the same side (of 2?)
          if (other.Plane.SameSide(this.Item1, this.Item2) && other.Plane.SameSide(this.Item1, this.Item3)) {
               return false;
          }
 
          // Compute the plane equation of triangle 
 
          // Reject as trivial if all points of triangle 2 are on the same side (of 1?)
          if (this.Plane.SameSide(other.Item1, other.Item2) && this.Plane.SameSide(other.Item1, other.Item3)) {
               return false;
          }
 
          // AT THIS POINT, TRIANGLES MUST BE TOUCHING SOMEWHERE
 
          // Compute intersection line (this line is infinite in both directions, the interval represents t)
           Vec3 L = Vec3.Cross(this.Plane.Normal, other.Plane.Normal);
 
          // Project vertices onto L
          double p1_1 = this.Item1.ScalarProjectionOnto(L);
          double p1_2 = this.Item2.ScalarProjectionOnto(L);
          double p1_3 = this.Item3.ScalarProjectionOnto(L);
 
          double p2_1 = other.Item1.ScalarProjectionOnto(L);
          double p2_2 = other.Item2.ScalarProjectionOnto(L);
          double p2_3 = other.Item3.ScalarProjectionOnto(L);
 
          // Compute the intervals for each triangle
          double T1Start = Math.Min(p1_1, Math.Min(p1_2, p1_3));
          double T1End = Math.Max(p1_1, Math.Max(p1_2, p1_3));
          double T2Start = Math.Min(p2_1, Math.Min(p2_2, p2_3));
          double T2End = Math.Max(p2_1, Math.Max(p2_2, p2_3));
 
          // Intersect the intervals, compute the intersection points
          if (Between(T2End, T1Start, T1End) || Between(T2Start, T1Start, T1End)) {
               return true;
          } else {
               return false;
          }
     }

     public override string ToString() {
        return String.Format("(a:{0},b:{1},c:{2})", Item1, Item2, Item3);
    }
}

}