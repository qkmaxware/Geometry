using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class ExpodeModifierTest : PrimitiveTest {
    [TestMethod]
    public void TestExpload() {
        IMesh geom1 = new Sphere(1, Vec3.Zero);

        SaveGeometry("sphere.explode.modifier", new Geometry.Modifiers.Explode(geom1, 4f));
    }

}

}