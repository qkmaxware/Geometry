using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class SubdivideModifierTest : PrimitiveTest {
    [TestMethod]
    public void SubdivideArray() {
        IEnumerable<Triangle> geom = new Cube(1, Vec3.Zero);
        geom = new Geometry.Modifiers.Subdivide(geom);
        SaveGeometry("subdivide.modifier", geom);
    }
}

}