using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class Box3Test {
    [TestMethod]
    public void TestIntersects() {
        Box3 bx1 = new Box3(new Vec3(-1,-1,-1), new Vec3(1,1,1));
        Box3 bx2 = new Box3(new Vec3(2,-1,-1), new Vec3(3,1,1));
        Box3 bx3 = new Box3(new Vec3(-1.5,-1,-1), new Vec3(1.5,1,1));

        Assert.AreEqual(true, bx1.Intersects(bx3));
        Assert.AreEqual(false, bx1.Intersects(bx2));
    }

    [TestMethod]
    public void TestIntersectTriangle() {
        Box3 sp1 = new Box3(new Vec3(-1,-1,-1), new Vec3(1,1,1));
        Triangle inT = new Triangle(new Vec3(-1,-1,0), new Vec3(0,1,0), new Vec3(1,1,0));
        Triangle outT = new Triangle(new Vec3(-1,-1,2), new Vec3(0,1,2), new Vec3(1,1,2));

        Assert.AreEqual(true, sp1.Intersects(inT));
        Assert.AreEqual(false, sp1.Intersects(outT));
    }

    [TestMethod]
    public void TestContains() {
        Box3 bx1 = new Box3(new Vec3(-1,-1,-1), new Vec3(1,1,1));
        Vec3 vx1 = new Vec3(0, 0, 0);
        Vec3 vx2 = new Vec3(4, 1, 1);

        Assert.AreEqual(true, bx1.Contains(vx1));
        Assert.AreEqual(false, bx1.Contains(vx2));
    }

    [TestMethod]
    public void TestMerge() {
        Box3 bx1 = new Box3(new Vec3(-1,-1,-1), new Vec3(1,1,1));
        Box3 bx2 = new Box3(new Vec3(2,-1,-1), new Vec3(3,1,1));

        Box3 bx3 = Box3.Merge(bx1, bx2);

        Assert.AreEqual(new Vec3(-1, -1, -1), bx3.Min);
        Assert.AreEqual(new Vec3(3, 1, 1), bx3.Max);
    }
}

}