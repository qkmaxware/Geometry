using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;

namespace Qkmaxware.Testing {

public class PrimitiveTest {
    public static void SaveGeometry(string name, Mesh mesh) {
        var exporter = new StlSerializer();

        if (!Directory.Exists(".data"))
            Directory.CreateDirectory(".data");
        using (var writer = new StreamWriter(Path.Combine(".data", $"{name}.stl"))) {
            writer.Write ( exporter.Serialize(mesh, binary: false) );
        }
    }
}

}