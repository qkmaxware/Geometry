using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class MirrorModifierTest : PrimitiveTest {
    [TestMethod]
    public void TestMirror() {
        var geom = new Cube(1, Vec3.Zero);
        IEnumerable<Triangle> stack = Transformation.Offset(Vec3.I * 2) * geom;
        stack = new Geometry.Modifiers.Mirror(Vec3.I, stack);
        SaveGeometry("mirror.modifier", stack);
    }
}

}