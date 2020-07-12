using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Qkmaxware.Geometry.IO {

public class StlSerializer {

    /// <summary>
    /// MIME type for binary STL files
    /// </summary>
    public static readonly string BinaryMIME = "model/x.stl-binary";

    /// <summary>
    /// MIME type for ascii STL files
    /// </summary>
    public static readonly string AsciiMIME = "model/x.stl-ascii";

    /// <summary>
    /// Write out an STL ASCII encoded string
    /// </summary>
    /// <param name="solid">solid to encode</param>
    public string Serialize(IEnumerable<Triangle> solid) {
        return SerializeAscii(solid);
    }

    /// <summary>
    /// Check if an STL file is written in ASCII format or not
    /// </summary>
    /// <param name="pathlike">path to file</param>
    /// <returns>true if file can be read as an ASCII STL file</returns>
    public bool IsStlAscii(string pathlike) {
        return File.Exists(pathlike) && File.ReadLines(pathlike).First().StartsWith("solid");
    }

    /// <summary>
    /// Write out an STL binary encoded file
    /// </summary>
    /// <param name="solid">solid to encode</param>
    /// <param name="writer">binary writer to write to</param>
    public void SerializeBinary(IEnumerable<Triangle> solid, BinaryWriter writer) {
        // Binary writer is in little-endian
        // Any 80 character header except solid, blank here
        writer.Write(new byte[80]);
        // Number of triangles
        writer.Write((UInt32)solid.Count());

        foreach(Triangle tri in solid) {
            Vec3 norm = tri.Plane.Normal;
            // Arrays of REAL32 values in iEEE format
            writer.Write((float)norm.X);writer.Write((float)norm.Y);writer.Write((float)norm.Z);
            writer.Write((float)tri.Item1.X);writer.Write((float)tri.Item1.Y);writer.Write((float)tri.Item1.Z);
            writer.Write((float)tri.Item2.X);writer.Write((float)tri.Item2.Y);writer.Write((float)tri.Item2.Z);
            writer.Write((float)tri.Item3.X);writer.Write((float)tri.Item3.Y);writer.Write((float)tri.Item3.Z);
            //Attribute count
            writer.Write((UInt16)0);
        }
        writer.Flush();
    }

    private string SerializeAscii(IEnumerable<Triangle> solid) {
        var writer = new StringWriter();
        using (writer) {
            writer.WriteLine("solid");
            foreach (Triangle tri in solid) {
                Vec3 norm = tri.Plane.Normal;
                writer.Write("facet normal ");writer.Write(norm.X);writer.Write(' ');writer.Write(norm.Y);writer.Write(' ');writer.WriteLine(norm.Z);
                writer.WriteLine(" outer loop");

                writer.Write("  vertex ");writer.Write(tri.Item1.X);writer.Write(' ');writer.Write(tri.Item1.Y);writer.Write(' ');writer.WriteLine(tri.Item1.Z);
                writer.Write("  vertex ");writer.Write(tri.Item2.X);writer.Write(' ');writer.Write(tri.Item2.Y);writer.Write(' ');writer.WriteLine(tri.Item2.Z);
                writer.Write("  vertex ");writer.Write(tri.Item3.X);writer.Write(' ');writer.Write(tri.Item3.Y);writer.Write(' ');writer.WriteLine(tri.Item3.Z);

                writer.WriteLine(" endloop");
                writer.WriteLine("endfacet");
            }
            writer.Write("endsolid");
            writer.Flush();
        }
        return writer.ToString();
    }

    private static Regex header = new Regex("solid(.*)?");
    private static Regex facet = new Regex("facet(.*)");
    private static Regex loop = new Regex("outer loop");
    private static Regex vertex = new Regex(
        @"vertex\s+([-+]?(?:[0-9]*\.[0-9]+|[0-9]+))\s+([-+]?(?:[0-9]*\.[0-9]+|[0-9]+))\s+([-+]?(?:[0-9]*\.[0-9]+|[0-9]+))"
    );
    private static Regex endLoop = new Regex("endloop");
    private static Regex endFacet = new Regex("endfacet");
    private static Regex footer = new Regex("endsolid(.*)?");

    /// <summary>
    /// Read an Binary Stl file
    /// </summary>
    /// <param name="reader">input binary</param>
    /// <returns>solid</returns>
    public Mesh Deserialize(BinaryReader reader) {
        // Read and ignore the header
        byte[] header = new byte[80];
        reader.Read(header, 0, header.Length);
        // Number of triangles
        UInt32 tris = reader.ReadUInt32();
        // Create triangle list
        List<Triangle> mylist = new List<Triangle>((int)tris);
        // Read each triangle
        for (int i = 0; i < tris; i++) {
            // Read normal
            Vec3 norm = ReadBinaryVec(reader);
            // Read p1
            Vec3 p1 = ReadBinaryVec(reader);
            // Read p2
            Vec3 p2 = ReadBinaryVec(reader);
            // Read p3
            Vec3 p3 = ReadBinaryVec(reader);
            // Read attribute count
            UInt16 attrs = reader.ReadUInt16();

            Triangle tri = new Triangle(p1,p2,p3);
            mylist.Add(tri); // For now, ignore normal
        }

        return new Mesh(mylist);
    }
    private static Vec3 ReadBinaryVec(BinaryReader reader) {
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();
        return new Vec3(x,y,z);
    }

    /// <summary>
    /// Read an ASCII Stl file
    /// </summary>
    /// <param name="reader">input text</param>
    /// <returns>solid</returns>
    public Mesh Deserialize(TextReader reader) {
        // String must start with solid
        string firstLine = reader.ReadLine();
        if (!firstLine.StartsWith("solid")) {
            throw new FormatException();
        }
        // Ignore name
        // Read facets
        List<Triangle> tris = new List<Triangle>();
        string line;
        while((line = reader.ReadLine()) != null) {
            // Match facet start, discard normal
            if (!facet.IsMatch(line)) {
                break;
            }
            
            // Read outer loop
            if (!loop.IsMatch(reader.ReadLine())) {
                throw new FormatException();
            }

            // Read all vertices
            List<Vec3> vertices = new List<Vec3>();
            string innerline;
            while((innerline = reader.ReadLine()) != null && vertex.IsMatch(innerline)) {
                Match match = vertex.Match(innerline);
                vertices.Add(
                    new Vec3(
                        double.Parse(match.Groups[1].Value),
                        double.Parse(match.Groups[2].Value),
                        double.Parse(match.Groups[3].Value)
                    )
                );
            }
            if (vertices.Count != 3) {
                throw new FormatException();
            }

            tris.Add(new Triangle(vertices[0], vertices[1], vertices[2]));

            // Read end loop
            if (!endLoop.IsMatch(innerline)) {
                throw new FormatException();
            }

            // Confirm end facet
            if (!endFacet.IsMatch(reader.ReadLine())) {
                throw new FormatException();
            }
        }

        return new Mesh(tris);
    }

}

}