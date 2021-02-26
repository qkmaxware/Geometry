using System.Collections.Generic;
using Qkmaxware.Geometry.Coordinates;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// InFrame modifier defines a mesh to be within a given frame of reference and will translate the geometry to global space when resolved
/// </summary>
public class InFrame : PolygonDeformationModifier {

    /// <summary>
    /// Frame of reference which the mesh is a member of
    /// </summary>
    /// <value>frame of reference</value>
    public Frame FrameOfReference {get; set;}

    /// <summary>
    /// Create InFrame modifier for the given geometry
    /// </summary>
    /// <param name="mesh">mesh contained within the frame of reference</param>
    /// <param name="reference">frame of reference</param>
    /// <returns>InFrame modifier</returns>
    public InFrame(IMesh mesh, Frame reference) : base(mesh) {
        this.FrameOfReference = reference;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        foreach (var tri in this.Original) {
            var v1 = this.FrameOfReference.LocalToGlobalPoint(tri.Item1);
            var v2 = this.FrameOfReference.LocalToGlobalPoint(tri.Item2);
            var v3 = this.FrameOfReference.LocalToGlobalPoint(tri.Item3);
            yield return new Triangle(v1, v2, v3);
        }
    }
}

}