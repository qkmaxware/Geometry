using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Modifier to deform existing geometry
/// </summary>
public abstract class DeformationModifier : GeneratorModifier<IEnumerable<Triangle>> {
    public DeformationModifier(IEnumerable<Triangle> triangles) : base (triangles) {}
}

}