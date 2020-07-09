using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class RayTest {
    [TestMethod]
    public void TestCast() {
        Ray ray1 = new Ray(Vec3.Zero, Vec3.K); // Ray pointing up
        Ray ray2 = new Ray(Vec3.Zero, -Vec3.K); // Ray pointing down

        //Triangle floating above the rays
        Triangle tri = new Triangle(
            new Vec3(-1, -1, 1), 
            new Vec3(0, 1, 1), 
            new Vec3(1, -1, 1)
        );

        Vec3 hit1;
        Vec3 hit2;
        Assert.AreEqual(true, ray1.Cast(tri, out hit1));
        Assert.AreEqual(new Vec3(0,0,1), hit1);
        Assert.AreEqual(false, ray2.Cast(tri, out hit2));
    }
}

}