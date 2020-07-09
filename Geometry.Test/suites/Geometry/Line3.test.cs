using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class Line3Test {
    [TestMethod]
    public void TestLength() {
        Line3 line = new Line3(new Vec3(-1,0,0), new Vec3(1,0,0));

        Assert.AreEqual(4, line.SqrLength);
        Assert.AreEqual(2, line.Length);
    }
}

}