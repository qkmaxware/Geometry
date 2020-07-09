using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class CapsuleTest : PrimitiveTest {
    [TestMethod]
    public void TestCapsule() {
        var geom = new Capsule(0.5, 2, Vec3.Zero, 16, 16);
        SaveGeometry("capsule", geom);
    }
}

}