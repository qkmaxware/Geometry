using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

[TestClass]
public class BooleanModifierTest : PrimitiveTest {
    [TestMethod]
    public void TestUnion() {
        IEnumerable<Triangle> geom1 = new Cube(1, Vec3.Zero);
        IEnumerable<Triangle> geom2 = new Cube(1, 0.5 * Vec3.One);

        SaveGeometry("boolean.union.modifier", new Geometry.Modifiers.Union(geom1, geom2));
    }

    [TestMethod]
    public void TestIntersection() {
        IEnumerable<Triangle> geom1 = new Cube(1, Vec3.Zero);
        IEnumerable<Triangle> geom2 = new Cube(1, 0.5 * Vec3.One);

        SaveGeometry("boolean.intersection.modifier", new Geometry.Modifiers.Intersection(geom1, geom2));
    }

    [TestMethod]
    public void TestDifference() {
        IEnumerable<Triangle> geom1 = new Cube(1, Vec3.Zero);
        IEnumerable<Triangle> geom2 = new Cube(1, 0.5 * Vec3.One);

        SaveGeometry("boolean.difference.modifier", new Geometry.Modifiers.Difference(geom1, geom2));
    }
}

}