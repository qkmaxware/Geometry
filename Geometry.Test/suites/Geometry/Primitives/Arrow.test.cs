using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class ArrowTest : PrimitiveTest {
    [TestMethod]
    public void TestArrow() {
        var geom = new Arrow(1, 0.08, 8);
        SaveGeometry("arrow", geom);
    }
}

}