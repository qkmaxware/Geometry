using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Qkmaxware.Geometry;

namespace Qkmaxware.Testing {

[TestClass]
public class NurbsCurveTest {
    [TestMethod]
    public void TestLine() {
        var curve = NurbsCurve.Line(Vec3.Zero, Vec3.One);

        // Verify intermediate points (evaluation works)
        Assert.AreEqual(new Vec3( 0,0,0), curve[0]  );
        Assert.AreEqual(new Vec3(0.5,0.5,0.5), curve[0.5]);
        Assert.AreEqual(new Vec3( 1,1,1), curve[1]  );
    }

    [TestMethod]
    public void TestCircle() {
        var curve = NurbsCurve.Circle(Vec3.Zero, 2);
        var knots = curve.KnotVector.ToList();

        // Checkout base properties
        Assert.AreEqual(2, curve.Degree); 
        Assert.AreEqual(3, curve.Order);
        Assert.AreEqual(12, knots.Count);

        // Verify intermediate points (evaluation works)
        Assert.AreEqual(new Vec3( 2,0,0), curve[0]  );
        Assert.AreEqual(new Vec3(-2,0,0), curve[0.5]);
        Assert.AreEqual(new Vec3( 2,0,0), curve[1]  );
    }

    [TestMethod]
    public void TestSpline() {
        var curve = new NurbsCurve(new NurbsControlPoint[]{
            new NurbsControlPoint(new Vec3(-4, -4, 0), 1),
            new NurbsControlPoint(new Vec3(-2, 4, 0), 1),
            new NurbsControlPoint(new Vec3(2, -4, 0), 1),
            new NurbsControlPoint(new Vec3(4, 4, 0), 1)
        }, NurbsPinning.PinBoth);
        var knots = curve.KnotVector.ToList();

        // Check out base properties
        Assert.AreEqual(4, curve.ControlPoints.Length);
        Assert.AreEqual(3, curve.Degree);
        Assert.AreEqual(4, curve.Order);
        Assert.AreEqual(8, knots.Count);

        // Check out knot values
        Assert.AreEqual(0, knots[0]);
        Assert.AreEqual(0, knots[1]);
        Assert.AreEqual(0, knots[2]);
        Assert.AreEqual(0, knots[3]);

        Assert.AreEqual(1, knots[4]);
        Assert.AreEqual(1, knots[5]);
        Assert.AreEqual(1, knots[6]);
        Assert.AreEqual(1, knots[7]);

        // Verify intermediate points (evaluation works)
        var centre = curve[0.5];
        Assert.AreEqual(0, centre.X, 0.01);
        Assert.AreEqual(0, centre.Y, 0.01);
        Assert.AreEqual(0, centre.Z, 0.01);

        var left = curve[0.25];
        Assert.AreEqual(-2.188, left.X, 0.01);
        Assert.AreEqual(-0.5, left.Y, 0.01);
        Assert.AreEqual(0, left.Z, 0.01);

        var right = curve[0.75];
        Assert.AreEqual(2.188, right.X, 0.01);
        Assert.AreEqual(0.5, right.Y, 0.01);
        Assert.AreEqual(0, right.Z, 0.01);
    }

}

}