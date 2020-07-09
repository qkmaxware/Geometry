using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class TorusTest : PrimitiveTest {
    [TestMethod]
    public void TestTorus() {
        var geom = new Torus(1, 0.2, Vec3.Zero);
        SaveGeometry("torus", geom);
    }
}

}