using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Coordinates;
using System;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class FrameTest {
    [TestMethod]
    public void TestOffset() {
        var F1 = new Frame(Quat.Identity, new Vec3( 1, 0, 0));
        var F2 = new Frame(Quat.Identity, new Vec3(-1, 0, 0));
        
        var v1 = Vec3.One;
        var v2 = F1.LocalToFramePoint(v1, F2);

        Assert.AreEqual(new Vec3(3, 1, 1).ToString(), v2.ToString());
    }

    [TestMethod]
    public void TestNestedOffset() {
        var F1 = new Frame(Quat.Identity, new Vec3(1, 0, 0));
        F1.Parent = new Frame(Quat.Identity, new Vec3(1, 0, 0));

        var v1 = Vec3.One;
        var v2 = F1.LocalToGlobalPoint(v1);

        Assert.AreEqual(new Vec3(3, 1, 1).ToString(), v2.ToString());
    }

    [TestMethod]
    public void TestRotation() {
        var F1 = new Frame(Quat.Identity, Vec3.Zero);
        var F2 = new Frame(Quat.AngleAxis(Vec3.K, -Math.PI/2), Vec3.Zero); // rotated 90deg

        var v1 = Vec3.One;
        var v2 = F1.LocalToFramePoint(v1, F2);

        Assert.AreEqual(new Vec3(-1, 1, 1).ToString(), v2.ToString());
    }

    [TestMethod]
    public void TestNestedRotation() {
        var F1 = new Frame(Quat.AngleAxis(Vec3.K, -Math.PI/2), Vec3.Zero);    // rotated 90deg
        F1.Parent = new Frame(Quat.AngleAxis(Vec3.K, -Math.PI/2), Vec3.Zero); // rotated 90deg

        var v1 = Vec3.One;
        var v2 = F1.LocalToGlobalPoint(v1);

        Assert.AreEqual(new Vec3(-1, -1, 1).ToString(), v2.ToString());
    }
}

}