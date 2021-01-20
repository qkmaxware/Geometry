using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class PathToMeshModifierTest : PrimitiveTest {
    [TestMethod]
    public void LinePath() {
        var path = new Line3(Vec3.Zero, new Vec3(0, 2, 0));
        IMesh geom = new Geometry.Modifiers.PathToMesh(path, radius: 0.2f, step: 0.25f);
        SaveGeometry("path2mesh.line.modifier", geom);
    }

    [TestMethod]
    public void CurvePath() {
        var path = new CubicBezierCurve(new Vec3(0, 0, 1), new Vec3(2, 0, 0), new Vec3(9, 0, 0),new Vec3(11, 0, 1));
        IMesh geom = new Geometry.Modifiers.PathToMesh(path, radius: 0.2f, step: 0.25f);
        SaveGeometry("path2mesh.curve.modifier", geom);
    }
}

}