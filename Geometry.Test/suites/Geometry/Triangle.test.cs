using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Astro.Testing {

[TestClass]
public class TriangleTest {
    [TestMethod]
    public void TestPlane() {
        Triangle tri = new Triangle(new Vec3(-1, 0, 0), new Vec3(0, 1, 0), new Vec3(1, 0, 0));

        Assert.AreEqual(new Plane(Vec3.K, 0), tri.Plane);
    }
    [TestMethod]
    public void TestIntersects() {
        Triangle vertical = new Triangle(
            new Vec3(-1,0,-1),
            new Vec3(0,0,1),
            new Vec3(1,0,-1)
        );
        Triangle horizontal_centred = new Triangle(
            new Vec3(-1,-1,0),
            new Vec3(0,1,0),
            new Vec3(1,-1,0)
        );
        Triangle horizontal_offcentre = new Triangle(
            new Vec3(-1,-3,0),
            new Vec3(0,-1,0),
            new Vec3(1,-3,0)
        );

        Assert.AreEqual(true, vertical.Intersects(horizontal_centred));
        Assert.AreEqual(false, vertical.Intersects(horizontal_offcentre));
    }

    [TestMethod]
    public void TestIntersection() {
        Triangle vertical = new Triangle(
            new Vec3(-1,0,-1),
            new Vec3(0,0,1),
            new Vec3(1,0,-1)
        );
        Triangle horizontal_centred = new Triangle(
            new Vec3(-1,-1,0),
            new Vec3(0,1,0),
            new Vec3(1,-1,0)
        );
        Triangle horizontal_offcentre = new Triangle(
            new Vec3(-1,-3,0),
            new Vec3(0,-1,0),
            new Vec3(1,-3,0)
        );

        Line3 intersection = new Line3(
            new Vec3(-0.5,0,0),
            new Vec3(0.5,0,0)
        );

        Line3 hit;

        Assert.AreEqual(true, vertical.Intersection(horizontal_centred, out hit));
        Assert.AreEqual(intersection, hit);
        Assert.AreEqual(false, vertical.Intersection(horizontal_offcentre, out hit));
    }

    [TestMethod]
    public void TestClosestPointTo() {
        Triangle tri = new Triangle(new Vec3(-1, 0, 0), new Vec3(0, 1, 0), new Vec3(1, 0, 0));

        Assert.AreEqual(new Vec3(-1, 0, 0), tri.ClosestPointTo(new Vec3(-6, 0, 0)));
        Assert.AreEqual(new Vec3(0, 1, 0), tri.ClosestPointTo(new Vec3(0, 6, 0)));
        Assert.AreEqual(new Vec3(1, 0, 0), tri.ClosestPointTo(new Vec3(6, 0, 0)));
    }
}

}