using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Astro.Testing {

[TestClass]
public class PlaneTest {
    [TestMethod]
    public void TestProject() {
        Vec3 p1 = new Vec3(4,3,2);
        Plane plane = new Plane(Vec3.K, 0);

        //X,Y Plane
        Vec3 r1 = plane.Project(p1);

        Assert.AreEqual(4, r1.X);
        Assert.AreEqual(3, r1.Y);
        Assert.AreEqual(0, r1.Z);
    }

    [TestMethod]
    public void TestDistanceBetween() {
        Vec3 p1 = new Vec3(4,3,2);
        Plane plane = new Plane(Vec3.K, 0);

        //X,Y Plane
        double r1 = plane.DistanceBetween(p1);

        Assert.AreEqual(2, r1);
    }

    [TestMethod]
    public void TestSide() {
        Vec3 p1 = new Vec3(4,3,2);
        Vec3 p2 = new Vec3(4,3,-2);

        Plane plane = new Plane(Vec3.K, 0);

        //X,Y Plane
        Plane.PlanarSide r1 = plane.Side(p1);
        Plane.PlanarSide r2 = plane.Side(p2);

        Assert.AreEqual(Plane.PlanarSide.Above, r1);
        Assert.AreEqual(Plane.PlanarSide.Below, r2);
    }

    [TestMethod]
    public void TestSameSide() {
        Vec3 p1 = new Vec3(4,3,2);
        Vec3 p2 = new Vec3(4,3,-2);
        Vec3 p3 = new Vec3(-3,1,4);

        Plane plane = new Plane(Vec3.K, 0);

        //X,Y Plane
        Assert.AreEqual(true, plane.SameSide(p1, p3));
        Assert.AreEqual(false, plane.SameSide(p1, p2));
    }
}

}