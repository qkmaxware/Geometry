using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class HemisphereTest : PrimitiveTest {
    [TestMethod]
    public void TestHemisphere() {
        var geom = new Hemisphere(1, Vec3.Zero, 16, 16);
        SaveGeometry("hemisphere", geom);
    }
}

}