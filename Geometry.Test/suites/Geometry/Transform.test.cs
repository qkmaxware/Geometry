using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Astro.Testing {

[TestClass]
public class TransformTest {

    [TestMethod]
    public void TestOffset() {
        var offset = Transformation.Offset(new Vec3(2, 4, 6));

        var test1 = offset * new Vec3(1, 2, 3);
        Assert.AreEqual(new Vec3(3, 6, 9), test1);
    }

    [TestMethod]
    public void TestScale() {
        var transformation = Transformation.Scale(new Vec3(2, 3, 4));

        var test1 = transformation * new Vec3(1, 2, 3);
        Assert.AreEqual(new Vec3(2, 6, 12), test1);
    }

}

}