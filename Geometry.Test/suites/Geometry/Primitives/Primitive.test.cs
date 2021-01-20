using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using System.Collections.Generic;

namespace Qkmaxware.Testing {

public class PrimitiveTest {
    public static void SaveGeometry(string name, IMesh mesh) {
        var exporter = new StlSerializer();

        if (!Directory.Exists(".data"))
            Directory.CreateDirectory(".data");

        using (var writer = new StreamWriter(Path.Combine(".data", $"{name}.ascii.stl"))) {
            writer.Write ( exporter.Serialize(mesh) );
        }

        using (var writer =  new BinaryWriter(File.Open(Path.Combine(".data", $"{name}.binary.stl"), FileMode.Create))) {
            exporter.SerializeBinary(mesh, writer);
        }
    }
}

}