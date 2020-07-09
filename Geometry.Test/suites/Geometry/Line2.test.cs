using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class Line2Test {
    [TestMethod]
    public void TestLength() {
        Line2 line = new Line2(new Vec2(-1,0), new Vec2(1,0));

        Assert.AreEqual(4, line.SqrLength);
        Assert.AreEqual(2, line.Length);
    }
}

}