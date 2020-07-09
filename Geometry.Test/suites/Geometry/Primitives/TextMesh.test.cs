using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class TextMeshTest : PrimitiveTest {
    [TestMethod]
    public void TestTextMesh() {
        var geom = new TextMesh("HELLO WORLD");
        SaveGeometry("text", geom);
    }
}

}