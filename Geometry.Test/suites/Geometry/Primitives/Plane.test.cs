using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class PrimitivePlaneTest : PrimitiveTest {
    [TestMethod]
    public void TestPlane() {
        var geom = new Qkmaxware.Geometry.Primitives.Plane(1, Vec3.Zero);
        SaveGeometry("plane", geom);
    }
}

}