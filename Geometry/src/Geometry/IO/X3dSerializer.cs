using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace Qkmaxware.Geometry.IO {

/// <summary>
/// Class for encoding solid information as an X3d file
/// </summary>
public class X3dSerializer {

    /// <summary>
    /// MIME type for OBJ files
    /// </summary>
    public static readonly string AsciiMIME = "model/x3d+xml";

    /// <summary>
    /// Write out a readable ascii X3d file
    /// </summary>
    /// <param name="writer">stream to write to</param>
    /// <param name="solid">solid to encode</param>
    public string Serialize(IEnumerable<Triangle> solid) {
        var writer = new StringWriter();
        using (writer) {
            // Create document
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration( "1.0", "UTF-8", null );
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore( xmlDeclaration, root );

            XmlDocumentType doctype = doc.CreateDocumentType("X3D", "ISO//Web3D//DTD X3D 3.2//EN", "http://www.web3d.org/specifications/x3d-3.2.dtd", null);
            doc.AppendChild(doctype);

            // Write header
            XmlElement x3d = doc.CreateElement(string.Empty, "X3D", string.Empty);
            XmlAttribute profile = doc.CreateAttribute("profile");
            profile.Value = "Interchange";
            x3d.Attributes.Append(profile);
            XmlAttribute version = doc.CreateAttribute("version");
            version.Value = "3.2";
            x3d.Attributes.Append(version);
            doc.AppendChild(x3d);

            XmlElement scene = doc.CreateElement(string.Empty, "Scene", string.Empty);
            x3d.AppendChild(scene);

            XmlElement shape = doc.CreateElement(string.Empty, "Shape", string.Empty);
            scene.AppendChild(shape);
            
            XmlElement faceSet = doc.CreateElement(string.Empty, "IndexedFaceSet", string.Empty);
            shape.AppendChild(faceSet);
            XmlAttribute coordIndex = doc.CreateAttribute("coordIndex");
            faceSet.Attributes.Append(coordIndex);
            coordIndex.Value = string.Empty;
            int i = 0; bool first = true;

            XmlElement coords = doc.CreateElement(string.Empty, "Coordinate", string.Empty);
            faceSet.AppendChild(coords);
            XmlAttribute point = doc.CreateAttribute("point");
            coords.Attributes.Append(point); 

            foreach (Triangle tri in solid) {
                coordIndex.Value += (!first ? " " : string.Empty) + (i++) + " " + (i++) + " " + (i++) + " -1"; //-1 means current face has ended

                point.Value += (!first ? " " : string.Empty) + tri.Item1.X + " " + tri.Item1.Y + " " + tri.Item1.Z + " " + 
                    tri.Item2.X + " " + tri.Item2.Y + " " + tri.Item2.Z + " " + 
                    tri.Item3.X + " " + tri.Item3.Y + " " + tri.Item3.Z;

                first = false;
            }

            writer.WriteLine(doc.OuterXml);
            writer.Flush();
        }
        return writer.ToString();
    }
}

}