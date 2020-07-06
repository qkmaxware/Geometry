using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Astro.Testing {

[TestClass]
public class ConeTest : PrimitiveTest {
    [TestMethod]
    public void TestCone() {
        var geom = new Cone(0.5, 1, Vec3.Zero);
        SaveGeometry("cone", geom);
    }
}

}