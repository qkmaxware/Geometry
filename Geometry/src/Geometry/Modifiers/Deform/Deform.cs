using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Lattice modifier to adjust vertices based on position from a control cage
/// </summary>
public class Deform : PolygonDeformationModifier {

    /// <summary>
    /// Deformation cage controlling vertice deformation
    /// </summary>
    /// <value>deformation cage</value>
    public DeformationCage ControlCage {get; set;}
    
    /// <summary>
    /// Power of the deformation
    /// </summary>
    /// <value>power</value>
    public float Power {get; set;}

    /// <summary>
    /// Create deformation modifier for the given mesh using the control cage
    /// </summary>
    /// <param name="mesh">mesh to deform</param>
    /// <param name="cage">deformation cage to control deformation</param>
    /// <param name="power">deformation power</param>
    /// <returns>deformation modifier</returns>
    public Deform(IMesh mesh, DeformationCage cage, float power = 1) : base(mesh) {
        this.ControlCage = cage;
        this.Power = power;
    }

    private Transformation GetTransformation(Vec3 position) {
        // Experience a pull from each vertex in the cage, closest vertices get more pull
        Vec3 displacement = Vec3.Zero;
        foreach (var vertex in this.ControlCage.BoundaryVertices) {
            var dir = vertex - position;
            var RR = dir.SqrLength;
            var f = Power / RR;
            displacement += dir * f;
        }
        return Transformation.Offset(displacement);
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        foreach  (var tri in this.Original) {
            yield return new Triangle(
                GetTransformation(tri.Item1) * tri.Item1,
                GetTransformation(tri.Item2) * tri.Item2,
                GetTransformation(tri.Item3) * tri.Item3
            );
        }
    }

}

}