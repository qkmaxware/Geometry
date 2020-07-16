using System;
using System.Linq;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Union modifier to combine non-overlapping mesh data
/// </summary>
public class Union : BaseBooleanModifier {

    /// <summary>
    /// Union of two solid's mesh data
    /// </summary>
    /// <param name="source1">initial mesh</param>
    /// <param name="source2">merge data</param>
    public Union(IEnumerable<Triangle> source1, IEnumerable<Triangle> source2) : base(source1, source2) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        PartClassification classes = Classify(this.OriginalMesh, this.OriginalMesh2);
        foreach (var tri in classes.AOutsideB.Concat(classes.BOutsideA)) {
            yield return tri;
        }
    }
}

/// <summary>
/// Difference modifier to subtract mesh data
/// </summary>
public class Difference : BaseBooleanModifier {
    /// <summary>
    /// Clip the given solid by another
    /// </summary>
    /// <param name="source">initial mesh</param>
    /// <param name="mask">clipping mask</param>
    public Difference(IEnumerable<Triangle> source, IEnumerable<Triangle> mask) : base(source, mask) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        PartClassification classes = Classify(this.OriginalMesh, this.OriginalMesh2);
        foreach (var tri in classes.AOutsideB.Concat(classes.BInsideA)){
            yield return tri;
        }
    }
}

/// <summary>
/// Intersection modifier to combine only overlapping geometry
/// </summary>
public class Intersection : BaseBooleanModifier {
    /// <summary>
    /// Intersection of two solid's geometry
    /// </summary>
    /// <param name="source">initial mesh</param>
    /// <param name="mask">comparison mesh</param>
    /// <returns>Solid with geometry that is the intersection of the input solids</returns>
    public Intersection(IEnumerable<Triangle> source, IEnumerable<Triangle> mask) : base(source, mask) {}

    public override IEnumerator<Triangle> GetEnumerator() {
        PartClassification classes = Classify(this.OriginalMesh, this.OriginalMesh2);
        foreach (var tri in classes.BInsideA.Concat(classes.AInsideB)) {
            yield return tri;
        }
    }
}

/// <summary>
/// Base class for boolean modifiers
/// </summary>
public abstract class BaseBooleanModifier : BaseModifier {
    /// <summary>
    /// Mask or joining mesh
    /// </summary>
    public IEnumerable<Triangle> OriginalMesh2 {get; set;}

    public BaseBooleanModifier(IEnumerable<Triangle> source1, IEnumerable<Triangle> source2) : base(source1) {
        this.OriginalMesh2 = source2;
    }

    // ---------------------------------------------------------------------------
    /*
     Boolean Operations Section
    */
    // ---------------------------------------------------------------------------
    private static Vec2 Collapse(Vec3 vector, Vec3 xaxis, Vec3 yaxis) {
        return new Vec2(
            Vec3.Dot(xaxis, vector),
            Vec3.Dot(yaxis, vector)
        );
    }

    private static bool IsEdge(double val) {
        return val > 0 && val < 1;
    }

    private static bool IsZeroOrOne(double val) {
        return 
            Math.Abs(val) < double.Epsilon || // Is Zero
            Math.Abs(val - 1) < double.Epsilon; // Is One
    }

    private static bool IsCorner(double val1, double val2) {
        return IsZeroOrOne(val1) && IsZeroOrOne(val2);
    }

    private static List<Triangle> Cut(Triangle tri, Line3 line) {
        // Create coordinate system
        Vec3 norm = tri.Normal;
        Vec3 xaxis = tri.Edge13.Normalized;
        Vec3 yaxis = Vec3.Cross(norm, xaxis);

        // Convert to 2d space
        Vec2 v1 = Vec2.Zero; // Consider point 0 the origin
        Vec2 v2 = Collapse(tri.Item2 - tri.Item1, xaxis, yaxis); // Relative position of Item2
        Vec2 v3 = Collapse(tri.Item3 - tri.Item1, xaxis, yaxis); // Relative position of Item3

        Vec2 ls = Collapse(line.Item1 - tri.Item1, xaxis, yaxis);
        Vec2 le = Collapse(line.Item2 - tri.Item1, xaxis, yaxis);

        Line2 l12 = new Line2(v1, v2);
        Line2 l13 = new Line2(v1, v3);
        Line2 l23 = new Line2(v2, v3);

        Line2 cut = new Line2(ls, le);

        // Perform intersection test
        double k12, k13, k23;
        double c12, c13, c23;
        bool i12 = l12.Intersects(cut, out c12, out k12);
        bool i13 = l13.Intersects(cut, out c13, out k13);
        bool i23 = l23.Intersects(cut, out c23, out k23);

        // Check cases
        List<Triangle> tris = new List<Triangle>();
        // Corner - Edge intersection (3 cases)
        if (IsCorner(c12,c13) && IsEdge(c23)) {
            // Crossing from corner 1 to edge 2->3
            Vec3 midpoint = Vec3.Lerp(tri.Item2, tri.Item3, c23);
            tris.Add(new Triangle(tri.Item1, tri.Item2, midpoint));
            tris.Add(new Triangle(tri.Item1, midpoint, tri.Item3));
            return tris;
        } else if (IsCorner(c13, c23) && IsEdge(c12)) {
            // Crossing from corner 3 to edge 1->2
            Vec3 midpoint = Vec3.Lerp(tri.Item1, tri.Item2, c12); 
            tris.Add(new Triangle(tri.Item1, midpoint, tri.Item3));
            tris.Add(new Triangle(midpoint, tri.Item2, tri.Item3));
            return tris;
        } else if (IsCorner(c12, c23) && IsEdge(c13)) {
            // Crossing from corner 2 to edge 1->3
            Vec3 midpoint = Vec3.Lerp(tri.Item1, tri.Item3, c13); 
            tris.Add(new Triangle(tri.Item1, tri.Item2, midpoint));
            tris.Add(new Triangle(midpoint, tri.Item2, tri.Item3));
            return tris;
        }
        // Edge - Edge intersection (3 cases)
        else if (IsEdge(c12) && IsEdge(c23)) {
            // Collision crossing 1->2, 2->3
            Vec3 m1 = Vec3.Lerp(tri.Item1, tri.Item2, c12);
            Vec3 m2 = Vec3.Lerp(tri.Item2, tri.Item3, c23);
            Vec3 mid = (tri.Item1 + tri.Item3) / 2;

            tris.Add(new Triangle(tri.Item1, m1, mid));
            tris.Add(new Triangle(m1, m2, mid));
            tris.Add(new Triangle(mid, m2, tri.Item3));

            tris.Add(new Triangle(m1, tri.Item2, m2));
            return tris;
        } else if (IsEdge(c12) && IsEdge(c13)) {
            // Collision crossing 1->3, 1->2
            Vec3 m1 = Vec3.Lerp(tri.Item1, tri.Item2, c12);
            Vec3 m2 = Vec3.Lerp(tri.Item1, tri.Item3, c13);
            Vec3 mid = (tri.Item2 + tri.Item3) / 2;

            tris.Add(new Triangle(m1, tri.Item2, mid));
            tris.Add(new Triangle(m1, mid, m2));
            tris.Add(new Triangle(m2, mid, tri.Item3));

            tris.Add(new Triangle(tri.Item1, m1, m2));
            return tris;
        }  else if (IsEdge(c13) && IsEdge(c23)) {
            // Collision crossing 1->3, 2->3
            Vec3 m1 = Vec3.Lerp(tri.Item1, tri.Item3, c13);
            Vec3 m2 = Vec3.Lerp(tri.Item2, tri.Item3, c23);
            Vec3 mid = (tri.Item1 + tri.Item2) / 2;

            tris.Add(new Triangle(mid, tri.Item2, m2));
            tris.Add(new Triangle(mid, m2, m1));
            tris.Add(new Triangle(tri.Item1, mid, m1));

            tris.Add(new Triangle(m1, m2, tri.Item3));
            return tris;
        }
        // No intersection (1 case)
        else {
            tris.Add(tri);
            return tris;
        }
    }

    private static List<Triangle> Cut(Triangle tri, List<Line3> lines) {
        List<Triangle> stack =  new List<Triangle>();
        stack.Add(tri);

        foreach(Line3 line in lines) {
            List<Triangle> cuts = new List<Triangle>();
            foreach (Triangle triangle in stack) {
                // Cut each triangle
                cuts.AddRange(Cut(triangle, line));
            }
            stack = cuts;
        }

        return stack;
    }

    protected class PartClassification {
        public List<Triangle> AInsideB     = new List<Triangle>();
        public List<Triangle> AOutsideB    = new List<Triangle>();
        public List<Triangle> BInsideA     = new List<Triangle>();
        public List<Triangle> BOutsideA    = new List<Triangle>();
    }

    // Classify parts of a solid as inside or outside another
    protected static PartClassification Classify(IEnumerable<Triangle> a, IEnumerable<Triangle> b) {
        // 0. Initialize intersection list
        List<List<Line3>> acuts = new List<List<Line3>>();
        List<List<Line3>> bcuts = new List<List<Line3>>();
        foreach (Triangle atri in a) {
            acuts.Add(new List<Line3>());
        }
        foreach (Triangle btri in b) {
            bcuts.Add(new List<Line3>());
        }
        
        // 1. Determine intersection lines O(N^2)
        Line3 hit; 
        // Iterate over Solid A
        int k = 0;
        foreach (Triangle atri in a) {
            // Create cut list for this triangle in A
            List<Line3> atriCuts = acuts[k++];
            int j = 0;
            // Iterate over Solid b
            foreach (Triangle btri in b) {
                // Create cut list for this triangle in B
                List<Line3> btriCuts = bcuts[j++];
                // Check if A & B intersect at these triangles
                if (atri.Intersection(btri, out hit)) {
                    // Add line as a cut line for both triangles
                    atriCuts.Add(hit);
                    btriCuts.Add(hit);
                }
            }
        }

        // 2. Split a mesh based on intersection lines O(2N)
        List<Triangle> SplitA = new List<Triangle>();
        List<Triangle> SplitB = new List<Triangle>();
        int i = 0;
        foreach(Triangle tri in a) {
            SplitA.AddRange(Cut(tri, acuts[i++]));
        }
        i = 0;
        foreach(Triangle tri in b) {
            SplitB.AddRange(Cut(tri, bcuts[i++]));
        }

        // 3. Classify triangles as AInsideB, AOutsideB, BInsideA, BOutsideA O(2N)
        // - you can use raycast (any dir) to determine if inside (odd hits) vs outside (even hits)
        PartClassification parts = new PartClassification();
        foreach (Triangle tri in SplitA) {
            Ray ray = new Ray(tri.Centre, tri.Normal);
            if (ray.CastAll(b).Count % 2 == 0) {
                // Even - Outside
                parts.AOutsideB.Add(tri);
            } else {
                // Odd - Inside
                parts.AInsideB.Add(tri);
            }
        }
        foreach (Triangle tri in SplitB) {
            Ray ray = new Ray(tri.Centre, tri.Normal);
            if (ray.CastAll(a).Count % 2 == 0) {
                // Even - Outside
                parts.BOutsideA.Add(tri);
            } else {
                // Odd - Inside
                parts.BInsideA.Add(tri);
            }
        }

        // 4. Return classifications
        return parts;
    }
}

}