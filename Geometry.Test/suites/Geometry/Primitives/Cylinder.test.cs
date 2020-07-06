using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Astro.Testing {

[TestClass]
public class CylinderTest : PrimitiveTest {
    [TestMethod]
    public void TestCylinder() {
        var geom = new Cylinder(0.5, 0.5, 1, Vec3.Zero);
        SaveGeometry("cylinder", geom);
    }
}

}