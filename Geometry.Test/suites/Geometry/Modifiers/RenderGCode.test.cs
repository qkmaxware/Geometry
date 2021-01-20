using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qkmaxware.Geometry;
using Qkmaxware.Geometry.IO;
using Qkmaxware.Geometry.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Qkmaxware.Testing {

[TestClass]
public class RenderGCodeModifierTest : PrimitiveTest {

    private string readGcodeFile(string filename) {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"Geometry.Test.gcode.{filename}";

        using (Stream stream = assembly.GetManifestResourceStream(resourceName)) {
            if (stream == null) {
                // Fallback if embedded stream can't find the file
                return File.ReadAllText(Path.Combine("..", "..", "..", "..", "Geometry.Test", "gcode", filename));
            } else {
                using (StreamReader reader = new StreamReader(stream)){
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
        }
    }

    [TestMethod]
    public void TestGCode() {
        var file = readGcodeFile("3DBenchy.gcode");
        var serializer = new GCodeSerializer();
        var gcode = serializer.Deserialize(new StringReader(file)).ToList();
        // File.WriteAllText(Path.Combine(".data", $"benchy.gcode"), GCodeSerializer.Serialize(gcode));
        var geom = new Geometry.Modifiers.RenderGCode(gcode, radius: 0.1f, resolution: 8);
        SaveGeometry("3dbenchy.gcode.modifier", geom);
    }
}

}