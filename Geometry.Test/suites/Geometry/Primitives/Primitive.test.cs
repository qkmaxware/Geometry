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

        ListMesh concreteMesh = new ListMesh(mesh); // concrete list mesh so we resolve modifiers once for both exporters

        using (var writer = new StreamWriter(Path.Combine(".data", $"{name}.ascii.stl"))) {
            writer.Write ( exporter.Serialize(concreteMesh) );
        }

        using (var writer =  new BinaryWriter(File.Open(Path.Combine(".data", $"{name}.binary.stl"), FileMode.Create))) {
            exporter.SerializeBinary(concreteMesh, writer);
        }
    }
}

}