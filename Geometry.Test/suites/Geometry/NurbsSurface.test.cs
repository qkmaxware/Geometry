using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class NurbsSurfaceModifierTest : PrimitiveTest {
    /*[TestMethod]
    public void TestNurbsPlane() {
        var i = NurbsCurve.Line(Vec3.Zero, Vec3.I);
        var j = NurbsCurve.Line(Vec3.Zero, Vec3.J);

        var surface = new NurbsSurface(i, j);

        Assert.AreEqual(new Vec3(0, 0, 0), surface[0, 0]);
        Assert.AreEqual(new Vec3(1, 0, 0), surface[1, 0]);
        Assert.AreEqual(new Vec3(0, 1, 0), surface[0, 1]);
        Assert.AreEqual(new Vec3(1, 1, 0), surface[1, 1]);
    }

    [TestMethod]
    public void TestNurbsCylinder() {
        var curve = NurbsCurve.Circle(Vec3.Zero, 1);
        var rail = NurbsCurve.Line(Vec3.Zero, Vec3.K);

        var surface = new NurbsSurface(curve, rail);

        for (double i = 0; i <= 1; i+=0.5) {
            Assert.AreEqual( new Vec3( 1,0,i), surface[0  , i] , $"Start coordinate at height {i}" );
            Assert.AreEqual( new Vec3(-1,0,i), surface[0.5, i] , $"Middle coordinate at height {i}" );
            Assert.AreEqual( new Vec3( 1,0,i), surface[1  , i] , $"End coordinate at height {i}" );
        }
    }*/

}

}