using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Modifier to deform existing geometry
/// </summary>
public abstract class PolygonDeformationModifier : PolygonGeneratorModifier<IEnumerable<Triangle>> {
    public PolygonDeformationModifier(IEnumerable<Triangle> triangles) : base (triangles) {}
}

}