using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Astro.Testing {

[TestClass]
public class CubeTest : PrimitiveTest {
    [TestMethod]
    public void TestCube() {
        var geom = new Cube(1, Vec3.Zero);
        SaveGeometry("cube", geom);
    }
}

}