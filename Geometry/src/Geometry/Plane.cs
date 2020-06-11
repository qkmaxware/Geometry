using System;

namespace Qkmaxware.Geometry {

    /// <summary>
    /// Represents an infinite plane in 3d-space
    /// </summary>
    public struct Plane : IEquatable<Plane> {
        /// <summary>
        /// Enum representing the two sides of an infinite plane
        /// </summary>
        public enum PlanarSide {
            Above, Below
        }

        /// <summary>
        /// XY plane
        /// </summary>
        /// <returns>plane</returns>
        public static readonly Plane XY = new Plane(Vec3.K, 0);
        /// <summary>
        /// XZ plane
        /// </summary>
        /// <returns>plane</returns>
        public static readonly Plane XZ = new Plane(Vec3.J, 0);
        /// <summary>
        /// YZ plane
        /// </summary>
        /// <returns>plane</returns>
        public static readonly Plane YZ = new Plane(Vec3.I, 0);

        /// <summary>
        /// The plane's normal vector
        /// </summary>
        /// <value>vector normal to the surface</value>
        public Vec3 Normal {get; private set;}

        /// <summary>
        /// Distance from the origin
        /// </summary>
        /// <value></value>
        public double Distance {get; private set;}

        /// <summary>
        /// Plane with its normal flipped
        /// </summary>
        /// <value></value>
        public Plane Flipped { 
            get {
                return new Plane(Normal.Flipped, -Distance);
            }
        }

        /// <summary>
        /// Create a plane from a normal and a distance
        /// </summary>
        /// <param name="normal">normalized vector</param>
        /// <param name="distance">distance from origin</param>
        public Plane(Vec3 normal, double distance) {
            this.Normal = normal.Normalized;
            this.Distance = distance;
        }

        /// <summary>
        /// Create a plane from a normal and a point on the plane
        /// </summary>
        /// <param name="normal">normalized vector</param>
        /// <param name="point">point in plane</param>
        public Plane(Vec3 normal, Vec3 point) {
            this.Normal = normal.Normalized;
            this.Distance = point.Length;
        }

        /// <summary>
        /// Create a plane on which all three points lie
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="b">vector</param>
        /// <param name="c">vector</param>
        public Plane(Vec3 a, Vec3 b, Vec3 c) {
            this.Normal = Vec3.Cross(c - a, b - a).Normalized;
            this.Distance = Vec3.Dot(Normal, a);
        }

        /// <summary>
        /// Compare two planes
        /// </summary>
        /// <param name="other">the other plane</param>
        /// <returns>true if planes are equal</returns>
        public bool Equals(Plane other) {
            return this.Normal.Equals(other.Normal) && this.Distance == (other.Distance);
        }

        /// <summary>
        /// Compare to object
        /// </summary>
        /// <param name="other">other object</param>
        /// <returns>true of objects are equal</returns>
        public override bool Equals(object other) {
            if (other is Plane) {
                return this.Equals((Plane)other);
            } else {
                return base.Equals(other);
            }
        }

        /// <summary>
        /// Get the hashcode for this object
        /// </summary>
        /// <returns>hash</returns>
        public override int GetHashCode() {
            return System.HashCode.Combine(Distance, Normal);
        }

        /// <summary>
        /// Project a point in space onto the plane
        /// </summary>
        /// <param name="point">point to project</param>
        /// <returns>projected point on the plane</returns>
        public Vec3 Project (Vec3 point) {
            var v = point - Normal * Distance;
            double dist = Vec3.Dot(Normal, v);
            return point - dist * Normal;
        }

        /// <summary>
        /// Closest point on this plane to the given position
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>closest point on plane</returns>
        public Vec3 ClosestPointTo(Vec3 position) {
            double distance = Vec3.Dot(Normal, position) - Distance;
            return position - distance * Normal;
        }

        /// <summary>
        /// Distance between a point an the surface of the plane
        /// </summary>
        /// <param name="point">point in space</param>
        /// <returns>distance from the surface</returns>
        public double DistanceBetween (Vec3 point) {
            var v = point - Normal * Distance;
            return Vec3.Dot(Normal, v);
        }

        /// <summary>
        /// Compute what side of a plane a point is over
        /// </summary>
        /// <param name="point">point in space</param>
        /// <returns>side of plane</returns>
        public PlanarSide Side(Vec3 point) {
            return DistanceBetween(point) > 0 ? PlanarSide.Above : PlanarSide.Below;
        } 

        /// <summary>
        /// Determine if two points in space lie on the same side of the plane
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <returns>true if both points are over the same side of the plane</returns>
        public bool SameSide(Vec3 a, Vec3 b) {
            return Side(a) == Side(b);
        }

        /// <summary>
        /// Convert plane to string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("(normal:{0},distance:{1})", Normal, Distance);
        } 
    }

}