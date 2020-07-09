using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class FrustumTest : PrimitiveTest {
    [TestMethod]
    public void TestFrustum() {
        var geom = new Frustum(1, 1, 0.5, Vec3.Zero, resolution: 4);
        SaveGeometry("frustum", geom);
    }
}

}