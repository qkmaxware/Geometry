using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class Vec3Test {
    [TestMethod]
    public void TestConstructor() {
        Vec3 vec = new Vec3(4,3,2);

        Assert.AreEqual(4, vec.X);
        Assert.AreEqual(3, vec.Y);
        Assert.AreEqual(2, vec.Z);
    }

    [TestMethod]
    public void TestEquals() {
        Vec3 a = new Vec3(4,3,2);
        Vec3 b = new Vec3(4,3,2);
        Vec3 c = new Vec3(2,3,4);

        Assert.AreEqual(true, a.Equals(b));
        Assert.AreEqual(false, a.Equals(c));
    }

    [TestMethod]
    public void TestLength() {
        Vec3 vec = new Vec3(4,3,2);

        Assert.AreEqual(29, vec.SqrLength);
        Assert.AreEqual(Math.Sqrt(29), vec.Length);
    }

    [TestMethod]
    public void TestUnit() {
        Vec3 unit = new Vec3(1, 0, 0);
        Vec3 vec = new Vec3(4, 0, 0);

        Assert.AreEqual(16, vec.SqrLength);
        Assert.AreEqual(unit, vec.Normalized);
    }

    [TestMethod]
    public void TestFlipped() {
        Vec3 vec = new Vec3(4, -2, 0);
        Vec3 f = vec.Flipped;

        Assert.AreEqual(-4, f.X);
        Assert.AreEqual(2, f.Y);
        Assert.AreEqual(0, f.Z);
    }

    [TestMethod]
    public void TestAbs() {
        Vec3 vec = new Vec3(4, -2, 0);
        Vec3 a = vec.Abs;

        Assert.AreEqual(4, a.X);
        Assert.AreEqual(2, a.Y);
        Assert.AreEqual(0, a.Z);
    }

    [TestMethod]
    public void TestReflect() {
        Vec3 normal = new Vec3(1,0,0);

        Vec3 vec = new Vec3(1, 1, 0);

        Assert.AreEqual(
            new Vec3(1,0,0), 
            vec.VectorProjectionOnto(normal)
        );

        Vec3 rel = vec.Reflect(normal);

        Assert.AreEqual(new Vec3(1,-1, 0), rel);
    }

    [TestMethod]
    public void TestVectorProjectionOnto() {
        Vec3 normal = new Vec3(1,0,0);

        Vec3 vec = new Vec3(1,1,0);
        Vec3 proj = vec.VectorProjectionOnto(normal);

        Assert.AreEqual(1, proj.X);
        Assert.AreEqual(0, proj.Y);
        Assert.AreEqual(0, proj.Z);
    }

    [TestMethod]
    public void TestScalarProjectionOnto() {
        Vec3 normal = new Vec3(1,0,0);

        Vec3 vec = new Vec3(1,1,0);
        double proj = vec.ScalarProjectionOnto(normal);

        Assert.AreEqual(1, proj);
    }

    [TestMethod]
    public void TestLerp() {
        Vec3 a = new Vec3(1,1,1);
        Vec3 b = new Vec3(2,2,2);

        Vec3 l1 = Vec3.Lerp(a,b, 0);
        Vec3 l2 = Vec3.Lerp(a,b, 1);
        Vec3 l3 = Vec3.Lerp(a,b, 0.5);

        Assert.AreEqual(a, l1);
        Assert.AreEqual(b, l2);
        Assert.AreEqual(new Vec3(1.5,1.5,1.5), l3);
    }

    [TestMethod]
    public void TestAngle() {
        Vec3 a = new Vec3(1,0,0);
        Vec3 b = new Vec3(0,1,0);
        Vec3 c = new Vec3(-1,0,0);

        Assert.AreEqual(0, Vec3.Angle(a,a));
        Assert.AreEqual(Math.PI, Vec3.Angle(a,c)); //180deg
        Assert.AreEqual(Math.PI / 2, Vec3.Angle(a,b)); // 90 deg
    }

    [TestMethod]
    public void TestSqrDistance() {
        Vec3 a = new Vec3(1,0,0);
        Vec3 b = new Vec3(4,0,0);

        Assert.AreEqual(9, Vec3.SqrDistance(a,b));
    }

    [TestMethod]
    public void TestDistance() {
        Vec3 a = new Vec3(1,0,0);
        Vec3 b = new Vec3(4,0,0);

        Assert.AreEqual(3, Vec3.Distance(a,b));
    }

    [TestMethod]
    public void TestDot() {
        Vec3 a = new Vec3(1,2,3);
        Vec3 b = new Vec3(6,5,4);

        Assert.AreEqual(28, Vec3.Dot(a,b));
    }

    [TestMethod]
    public void TestCross() {
        Assert.AreEqual(Vec3.K, Vec3.Cross(Vec3.I,Vec3.J));
        Assert.AreEqual(-Vec3.K, Vec3.Cross(Vec3.J,Vec3.I));

        Assert.AreEqual(-Vec3.I, Vec3.Cross(Vec3.K,Vec3.J));
        Assert.AreEqual(Vec3.I, Vec3.Cross(Vec3.J,Vec3.K));

        Assert.AreEqual(-Vec3.J, Vec3.Cross(Vec3.I,Vec3.K));
        Assert.AreEqual(Vec3.J, Vec3.Cross(Vec3.K,Vec3.I));
    }

    [TestMethod]
    public void TestMax() {
        Vec3 a = new Vec3(1,2,3);
        Vec3 b = new Vec3(3,2,1);

        Vec3 max = Vec3.Max(a,b);

        Assert.AreEqual(3, max.X);
        Assert.AreEqual(2, max.Y);
        Assert.AreEqual(3, max.Z);
    }

    [TestMethod]
    public void TestMin() {
        Vec3 a = new Vec3(1,2,3);
        Vec3 b = new Vec3(3,2,1);

        Vec3 min = Vec3.Min(a,b);

        Assert.AreEqual(1, min.X);
        Assert.AreEqual(2, min.Y);
        Assert.AreEqual(1, min.Z);
    }

    [TestMethod]
    public void TestAdd() {
        Vec3 a = new Vec3(1,2,3);
        Vec3 b = new Vec3(3,2,1);

        Vec3 r = a + b;

        Assert.AreEqual(4, r.X);
        Assert.AreEqual(4, r.Y);
        Assert.AreEqual(4, r.Z);
    }

    [TestMethod]
    public void TestSub() {
        Vec3 a = new Vec3(1,2,3);
        Vec3 b = new Vec3(3,2,1);

        Vec3 r = a - b;

        Assert.AreEqual(-2, r.X);
        Assert.AreEqual(0, r.Y);
        Assert.AreEqual(2, r.Z);
    }

    [TestMethod]
    public void TestNegate() {
        Vec3 a = new Vec3(1,2,3);

        Assert.AreEqual(a.Flipped, -a);
    }

    [TestMethod]
    public void TestScale() {
        Vec3 a = new Vec3(1,2,4);

        Vec3 b = 2 * a;
        Vec3 c = a * 3;
        Vec3 d = a / 2;

        Assert.AreEqual(2, b.X);
        Assert.AreEqual(4, b.Y);
        Assert.AreEqual(8, b.Z);

        Assert.AreEqual(3, c.X);
        Assert.AreEqual(6, c.Y);
        Assert.AreEqual(12, c.Z);

        Assert.AreEqual(0.5, d.X);
        Assert.AreEqual(1, d.Y);
        Assert.AreEqual(2, d.Z);
    }
}

}