using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class QuatTest {
    [TestMethod]
    public void TestConstructor() {
        var q = new Quat(1, 2, 3, 4);

        Assert.AreEqual(1, q.X);
        Assert.AreEqual(2, q.Y);
        Assert.AreEqual(3, q.Z);
        Assert.AreEqual(4, q.W);
    }

    [TestMethod]
    public void TestEquals() {
        var a = new Quat(4, 3, 2, 1);
        var b = new Quat(4, 3, 2, 1);
        var c = new Quat(2, 3, 4, 1);

        Assert.AreEqual(true, a.Equals(b));
        Assert.AreEqual(false, a.Equals(c));
    }

    [TestMethod]
    public void TestLength() {
        var vec = new Quat(4, 3, 2, 1);

        Assert.AreEqual(30, vec.SqrLength);
        Assert.AreEqual(Math.Sqrt(30), vec.Length);
    }

    [TestMethod]
    public void TestUnit() {
        var unit = new Quat(1, 0, 0, 0);
        var vec = new Quat(4, 0, 0, 0);

        Assert.AreEqual(16, vec.SqrLength);
        Assert.AreEqual(unit, vec.Normalized);
    }

    [TestMethod]
    public void TestFlipped() {
        var vec = new Quat(4, -2, 0, -1);
        var f = vec.Flipped;

        Assert.AreEqual(-4, f.X);
        Assert.AreEqual(2, f.Y);
        Assert.AreEqual(0, f.Z);
        Assert.AreEqual(1, f.W);
    }

    [TestMethod]
    public void TestAbs() {
        var vec = new Quat(4, -2, 0, -1);
        var a = vec.Abs;

        Assert.AreEqual(4, a.X);
        Assert.AreEqual(2, a.Y);
        Assert.AreEqual(0, a.Z);
        Assert.AreEqual(1, a.W);
    }

    [TestMethod]
    public void TestLerp() {
        Quat a = new Quat(1,1,1,1);
        Quat b = new Quat(2,2,2,2);

        Quat l1 = Quat.Lerp(a,b, 0);
        Quat l2 = Quat.Lerp(a,b, 1);
        Quat l3 = Quat.Lerp(a,b, 0.5);

        Assert.AreEqual(a, l1);
        Assert.AreEqual(b, l2);
        Assert.AreEqual(new Quat(1.5,1.5,1.5,1.5), l3);
    }

    [TestMethod]
    public void TestAdd() {
        Quat a = new Quat(1, 2, 3, 4);
        Quat b = new Quat(3, 2, 1, 4);

        Quat r = a + b;

        Assert.AreEqual(4, r.X);
        Assert.AreEqual(4, r.Y);
        Assert.AreEqual(4, r.Z);
        Assert.AreEqual(8, r.W);
    }

    [TestMethod]
    public void TestSub() {
        Quat a = new Quat(1, 2, 3, 4);
        Quat b = new Quat(3, 2, 1, 4);

        Quat r = a - b;

        Assert.AreEqual(-2, r.X);
        Assert.AreEqual(0, r.Y);
        Assert.AreEqual(2, r.Z);
        Assert.AreEqual(0, r.W);
    }

    [TestMethod]
    public void TestNegate() {
        Quat a = new Quat(1, 2, 3, 4);

        Assert.AreEqual(a.Flipped, -a);
    }

    [TestMethod]
    public void TestScale() {
        Quat a = new Quat(1, 2, 4, 6);

        Quat b = 2 * a;
        Quat c = a * 3;
        Quat d = a / 2;

        Assert.AreEqual(2, b.X);
        Assert.AreEqual(4, b.Y);
        Assert.AreEqual(8, b.Z);
        Assert.AreEqual(12, b.W);

        Assert.AreEqual(3, c.X);
        Assert.AreEqual(6, c.Y);
        Assert.AreEqual(12, c.Z);
        Assert.AreEqual(18, c.W);

        Assert.AreEqual(0.5, d.X);
        Assert.AreEqual(1, d.Y);
        Assert.AreEqual(2, d.Z);
        Assert.AreEqual(3, d.W);
    }
}

}