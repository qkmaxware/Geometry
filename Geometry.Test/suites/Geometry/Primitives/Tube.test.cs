using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Astro.Testing {

[TestClass]
public class TubeTest : PrimitiveTest {
    [TestMethod]
    public void TestTube() {
        var geom = new Tube(1, 0.8, 2, Vec3.Zero);
        SaveGeometry("tube", geom);
    }
}

}