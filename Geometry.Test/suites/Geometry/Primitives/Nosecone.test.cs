using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

[TestClass]
public class NoseconeTest : PrimitiveTest{
    [TestMethod]
    public void TestConic () {
        var exporter = new StlSerializer();
        var nosecone = new ConicNosecone(1, 2);
        
        SaveGeometry("nosecone.conic", nosecone);
    }

    [TestMethod]
    public void TestBiconic () {
        var exporter = new StlSerializer();
        var nosecone = new BiConicNosecone(0.5, 1, 0.7, 2);
        
        SaveGeometry("nosecone.biconic", nosecone);
    }

    [TestMethod]
    public void TestTangentOgive () {
        var exporter = new StlSerializer();
        var nosecone = new TangentOgiveNosecone(1, 2);
        
        SaveGeometry("nosecone.tangent", nosecone);
    }

    [TestMethod]
    public void TestSecantOgive () {
        var exporter = new StlSerializer();
        var nosecone = new SecantOgiveNosecone(2, 0.5, 1);
        
        SaveGeometry("nosecone.secant", nosecone);
    }

    [TestMethod]
    public void TestElliptical () {
        var exporter = new StlSerializer();
        var nosecone = new EllipticalNosecone(0.5, 2);
        
        SaveGeometry("nosecone.elliptical", nosecone);
    }

    [TestMethod]
    public void TestParabolic () {
        var exporter = new StlSerializer();
        var nosecone = new ParabolicNosecone(0.75, 0.5, 2);
        
        SaveGeometry("nosecone.parabolic", nosecone);
    }

    [TestMethod]
    public void TestPowerseries () {
        var exporter = new StlSerializer();
        var nosecone = new PowerseriesNosecone(0.5, 0.5, 2);
        
        SaveGeometry("nosecone.power", nosecone);
    }

    [TestMethod]
    public void TestHaack () {
        var exporter = new StlSerializer();
        var nosecone = new HaackNosecone(0.6, 0.5, 2);
        
        SaveGeometry("nosecone.haack", nosecone);
    }
}

}