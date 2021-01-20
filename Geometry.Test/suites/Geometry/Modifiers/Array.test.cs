using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class ArrayModifierTest : PrimitiveTest {
    [TestMethod]
    public void TestArray() {
        IMesh geom = new Cube(1, Vec3.Zero);
        geom = new Geometry.Modifiers.Array(geom, 4, new Vec3(1.5));
        SaveGeometry("array.modifier", geom);
    }
}

}