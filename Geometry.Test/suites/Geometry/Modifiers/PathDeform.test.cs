using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class PathDeformModifierTest : PrimitiveTest {
    [TestMethod]
    public void TestPathDeform() {
        IMesh geom = new TextMesh("Hello World");
        var curve = new CubicBezierCurve(new Vec3(0, 0, 1), new Vec3(2, 0, 0), new Vec3(9, 0, 0),new Vec3(11, 0, 1));
        geom = new Geometry.Modifiers.PathDeform(Vec3.I, curve, geom);

        MeshGroup group = new MeshGroup();
        group.Add(geom);
        SaveGeometry("pathdeform.modifier", group);
    }
}

}