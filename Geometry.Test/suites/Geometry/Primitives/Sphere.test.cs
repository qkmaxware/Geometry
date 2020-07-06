using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Astro.Testing {

[TestClass]
public class SphereTest : PrimitiveTest {
    [TestMethod]
    public void TestSphere() {
        var geom = new Sphere(1, Vec3.Zero, horizontalResolution: 32, verticalResolution: 32);
        SaveGeometry("sphere", geom);
    }
}

}