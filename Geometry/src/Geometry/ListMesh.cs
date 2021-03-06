using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qkmaxware.Geometry {

/// <summary>
/// A solid is a collection of triangles in 3 space
/// </summary>
public class ListMesh : IMesh {
    /// <summary>
    /// List of all triangles in the solid
    /// </summary>
    private List<Triangle> triangles;

    /// <summary>
    /// Count of the number of triangular faces
    /// </summary>
    /// <value>number of triangles</value>
    public int Count {
        get {
            return triangles != null ? triangles.Count : 0;
        }
    }

    private Box3? box = null;
    /// <summary>
    /// Compute a bounding volume for this solid
    /// </summary>
    /// <value>box bounding all triangles</value>
    public Box3 Bounds {
        get {
            if (box == null) {
                box = new Box3(this);
                
            } 
            return box;
        }
    }

    /// <summary>
    /// Compute the surface area of this mesh
    /// </summary>
    /// <returns>sum of all triangular areas</returns>
    public double SurfaceArea => this.Select(tri => tri.Area).Sum();

    /// <summary>
    /// Empty solid
    /// </summary>
    public ListMesh() {
        triangles = new List<Triangle>();
    }

    /// <summary>
    /// Solid composed of the given triangles
    /// </summary>
    /// <param name="triangles"></param>
    public ListMesh(IEnumerable<Triangle> triangles) {
        if (triangles == null) {
            this.triangles = new List<Triangle>();
        } else {
            this.triangles = new List<Triangle>(triangles);
        }
    }

    /// <summary>
    /// Add triangle to this mesh
    /// </summary>
    /// <param name="triangle">triangle</param>
    protected void Append (Triangle triangle) {
        this.triangles.Add(triangle);
    }

    /// <summary>
    /// Add triangles to this mesh
    /// </summary>
    /// <param name="triangles">several triangles</param>
    protected void AppendRange(IEnumerable<Triangle> triangles) {
        this.triangles.AddRange(triangles);
    }

    /// <summary>
    /// Create a new mesh by joining the triangles to another 
    /// </summary>
    /// <param name="other">mesh to join with</param>
    /// <returns>mesh with the triangles of both joined meshes</returns>
    public ListMesh Join (ListMesh other) {
        return new ListMesh(this.Concat(other));
    }

    /// <summary>
    /// Apply a transformation to this mesh's triangles
    /// </summary>
    /// <param name="matrix">transformation matrix</param>
    /// <returns>new transformed mesh</returns>
    public ListMesh Transform (Transformation matrix) {
        List<Triangle> new_tris = new List<Triangle>(this.triangles.Count);

        foreach (var tri in this.triangles) {
            new_tris.Add(tri.Transform(matrix));
        }

        return new ListMesh(new_tris);
    }

    /// <summary>
    /// Clear all triangles
    /// </summary>
    protected void Clear() {
        this.triangles.Clear();
    }

    /// <summary>
    /// Get an enumerator over all triangles in this solid
    /// </summary>
    /// <returns>typed IEnumerator over all triangles</returns>
    public IEnumerator<Triangle> GetEnumerator() {
        return (IEnumerator<Triangle>)triangles.GetEnumerator();
    }

    /// <summary>
    /// Get an enumerator over all triangles in this solid
    /// </summary>
    /// <returns>IEnumerator over all triangles</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return (IEnumerator)triangles.GetEnumerator();
    }

    /// <summary>
    /// Clip the given solid by another
    /// </summary>
    /// <param name="other">original mesh</param>
    /// <returns>clipped solid</returns>
    public ListMesh Difference (ListMesh other) {
        return new ListMesh (new Modifiers.Difference(this, other));
    }

    /// <summary>
    /// Clip the given solid by another
    /// </summary>
    /// <param name="b">clipping mask</param>
    /// <param name="a">original mesh</param>
    /// <returns>clipped solid</returns>
    public static ListMesh operator - (ListMesh a, ListMesh b) {
        return a.Difference(b);
    }

    /// <summary>
    /// Intersection of two solid's geometry
    /// </summary>
    /// <param name="other">comparision solid</param>
    /// <returns>Solid with geometry that is the intersection of the input solids</returns>
    public ListMesh Intersection (ListMesh other) {
        return new ListMesh (new Modifiers.Intersection(this, other));
    } 

    /// <summary>
    /// Intersection of two solid's geometry
    /// </summary>
    /// <param name="a">source solid</param>
    /// <param name="b">comparision solid</param>
    /// <returns>Solid with geometry that is the intersection of the input solids</returns>
    public static ListMesh operator & (ListMesh a, ListMesh b) {
        return a.Intersection(b);
    }

    /// <summary>
    /// Union of two solid's mesh data
    /// </summary>
    /// <param name="other">merge data</param>
    /// <returns>union of both solids</returns>
    public ListMesh Union (ListMesh other) {
        return new ListMesh (new Modifiers.Union(this, other));
    }

    /// <summary>
    /// Union of two solid's mesh data
    /// </summary>
    /// <param name="a">source mesh</param>
    /// <param name="b">merge data</param>
    /// <returns>union of both solids</returns>
    public static ListMesh operator + (ListMesh a, ListMesh b) {
        return a.Union(b);
    }

}

}