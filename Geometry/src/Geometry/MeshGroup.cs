using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry {

/// <summary>
/// Represents a grouping of meshes that can be transformed without modifying the mesh data and can be stacked
/// </summary>
public class MeshGroup : IMesh {
    /// <summary>
    /// List of contained meshes
    /// </summary>
    private List<IMesh> meshes = new List<IMesh>();
    /// <summary>
    /// The transformation to apply to the underlying mesh data
    /// </summary>
    public Transformation Transformation {get; set;} = Transformation.Identity();
    /// <summary>
    /// All sub mesh groups contained within this group
    /// </summary>
    /// <returns>Enumerable of all sub-groups</returns>
    public IEnumerable<MeshGroup> SubGroups => meshes.OfType<MeshGroup>();

    /// <summary>
    /// Zero argument default constructor
    /// </summary>
    public MeshGroup() {}

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="other">group to copy</param>
    public MeshGroup (MeshGroup other) {
        this.Transformation = other.Transformation;
        this.meshes = new List<IMesh>(this.meshes);
    }

    /// <summary>
    /// Add a mesh to the group
    /// </summary>
    /// <param name="mesh">mesh to add</param>
    public void Add(IMesh mesh) {
        this.meshes.Add(mesh);
    }

    /// <summary>
    /// Remove a mesh from the group
    /// </summary>
    /// <param name="mesh">mesh to remove</param>
    public void Remove(IMesh mesh) {
        this.meshes.Remove(mesh);
    }

    /// <summary>
    /// Clear the group
    /// </summary>
    public void Clear() {
        this.meshes.Clear();
    }

    /// <summary>
    /// Test if this group contains this mesh
    /// </summary>
    /// <param name="mesh">mesh to test</param>
    /// <returns>true if the group contains the mesh</returns>
    public bool Contains(IMesh mesh) {
        return this.meshes.Contains(mesh);
    }

    /// <summary>
    /// Apply a transformation on top of the existing transformation
    /// </summary>
    /// <param name="matrix">transformation to apply</param>
    public void Transform (Transformation matrix) {
        this.Transformation = matrix * this.Transformation;
    }

    /// <summary>
    /// Apply a transformation on top of the existing transformation
    /// </summary>
    /// <param name="matrix">transformation matrix</param>
    /// <param name="mesh">mesh group to change</param>
    /// <returns>mesh group with updated transformation</returns>
    public static MeshGroup operator * (Transformation matrix, MeshGroup mesh) {
        mesh.Transform(matrix);
        return mesh;
    }

    public IEnumerator<Triangle> GetEnumerator() {
        foreach (var mesh in meshes) {
            foreach (var tri in mesh) {
                yield return tri.Transform(this.Transformation);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Create a new mesh from this group applying the tranformation to the resultant triangles
    /// </summary>
    /// <param name="group">group to convert</param>
    public static explicit operator ListMesh (MeshGroup group) {
        return new ListMesh((IMesh)group);
    }
}

}