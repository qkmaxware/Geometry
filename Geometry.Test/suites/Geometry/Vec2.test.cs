using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class Vec2Test {
    [TestMethod]
    public void TestConstructor() {
        Vec2 vec = new Vec2(4,3);

        Assert.AreEqual(4, vec.X);
        Assert.AreEqual(3, vec.Y);
    }

    [TestMethod]
    public void TestEquals() {
        Vec2 a = new Vec2(4,3);
        Vec2 b = new Vec2(4,3);
        Vec2 c = new Vec2(2,3);

        Assert.AreEqual(true, a.Equals(b));
        Assert.AreEqual(false, a.Equals(c));
    }

    [TestMethod]
    public void TestLength() {
        Vec2 vec = new Vec2(4,3);

        Assert.AreEqual(25, vec.SqrLength);
        Assert.AreEqual(5, vec.Length);
    }

    [TestMethod]
    public void TestUnit() {
        Vec2 unit = new Vec2(1, 0);
        Vec2 vec = new Vec2(4, 0);

        Assert.AreEqual(16, vec.SqrLength);
        Assert.AreEqual(unit, vec.Normalized);
    }

    [TestMethod]
    public void TestFlipped() {
        Vec2 vec = new Vec2(4, -2);
        Vec2 f = vec.Flipped;

        Assert.AreEqual(-4, f.X);
        Assert.AreEqual(2, f.Y);
    }

    [TestMethod]
    public void TestAbs() {
        Vec2 vec = new Vec2(4, -2);
        Vec2 a = vec.Abs;

        Assert.AreEqual(4, a.X);
        Assert.AreEqual(2, a.Y);
    }

    [TestMethod]
    public void TestReflect() {
        Vec2 normal = new Vec2(1,0);

        Vec2 vec = new Vec2(1, 1);

        Assert.AreEqual(
            new Vec2(1,0), 
            vec.VectorProjectionOnto(normal)
        );

        Vec2 rel = vec.Reflect(normal);

        Assert.AreEqual(new Vec2(1,-1), rel);
    }

    [TestMethod]
    public void TestVectorProjectionOnto() {
        Vec2 normal = new Vec2(1,0);

        Vec2 vec = new Vec2(1,1);
        Vec2 proj = vec.VectorProjectionOnto(normal);

        Assert.AreEqual(1, proj.X);
        Assert.AreEqual(0, proj.Y);
    }

    [TestMethod]
    public void TestScalarProjectionOnto() {
        Vec2 normal = new Vec2(1,0);

        Vec2 vec = new Vec2(1,1);
        double proj = vec.ScalarProjectionOnto(normal);

        Assert.AreEqual(1, proj);
    }

    [TestMethod]
    public void TestLerp() {
        Vec2 a = new Vec2(1,1);
        Vec2 b = new Vec2(2,2);

        Vec2 l1 = Vec2.Lerp(a,b, 0);
        Vec2 l2 = Vec2.Lerp(a,b, 1);
        Vec2 l3 = Vec2.Lerp(a,b, 0.5);

        Assert.AreEqual(a, l1);
        Assert.AreEqual(b, l2);
        Assert.AreEqual(new Vec2(1.5,1.5), l3);
    }

    [TestMethod]
    public void TestAngle() {
        Vec2 a = new Vec2(1,0);
        Vec2 b = new Vec2(0,1);
        Vec2 c = new Vec2(-1,0);

        Assert.AreEqual(0, Vec2.Angle(a,a));
        Assert.AreEqual(Math.PI, Vec2.Angle(a,c)); //180deg
        Assert.AreEqual(Math.PI / 2, Vec2.Angle(a,b)); // 90 deg
    }


    [TestMethod]
    public void TestSqrDistance() {
        Vec2 a = new Vec2(1,0);
        Vec2 b = new Vec2(4,0);

        Assert.AreEqual(9, Vec2.SqrDistance(a,b));
    }

    [TestMethod]
    public void TestDistance() {
        Vec2 a = new Vec2(1,0);
        Vec2 b = new Vec2(4,0);

        Assert.AreEqual(3, Vec2.Distance(a,b));
    }

    [TestMethod]
    public void TestDot() {
        Vec2 a = new Vec2(1,2);
        Vec2 b = new Vec2(6,5);

        Assert.AreEqual(16, Vec2.Dot(a,b));
    }

    [TestMethod]
    public void TestMax() {
        Vec2 a = new Vec2(1,2);
        Vec2 b = new Vec2(3,2);

        Vec2 max = Vec2.Max(a,b);

        Assert.AreEqual(3, max.X);
        Assert.AreEqual(2, max.Y);
    }

    [TestMethod]
    public void TestMin() {
        Vec2 a = new Vec2(1,2);
        Vec2 b = new Vec2(3,2);

        Vec2 min = Vec2.Min(a,b);

        Assert.AreEqual(1, min.X);
        Assert.AreEqual(2, min.Y);
    }

    [TestMethod]
    public void TestAdd() {
        Vec2 a = new Vec2(1,2);
        Vec2 b = new Vec2(3,2);

        Vec2 r = a + b;

        Assert.AreEqual(4, r.X);
        Assert.AreEqual(4, r.Y);
    }

    [TestMethod]
    public void TestSub() {
        Vec2 a = new Vec2(1,2);
        Vec2 b = new Vec2(3,2);

        Vec2 r = a - b;

        Assert.AreEqual(-2, r.X);
        Assert.AreEqual(0, r.Y);
    }

    [TestMethod]
    public void TestNegate() {
        Vec2 a = new Vec2(1,2);

        Assert.AreEqual(a.Flipped, -a);
    }

    [TestMethod]
    public void TestScale() {
        Vec2 a = new Vec2(1,2);

        Vec2 b = 2 * a;
        Vec2 c = a * 3;
        Vec2 d = a / 2;

        Assert.AreEqual(2, b.X);
        Assert.AreEqual(4, b.Y);

        Assert.AreEqual(3, c.X);
        Assert.AreEqual(6, c.Y);

        Assert.AreEqual(0.5, d.X);
        Assert.AreEqual(1, d.Y);
    }

}

}