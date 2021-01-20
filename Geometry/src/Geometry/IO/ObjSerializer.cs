using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Qkmaxware.Geometry.IO {

/// <summary>
/// Class for encoding solid information as a wavefront OBJ file
/// </summary>
public class ObjSerializer {

    /// <summary>
    /// MIME type for OBJ files
    /// </summary>
    public static readonly string AsciiMIME = "text/plain";

    private static void WriteVector(string prefix, TextWriter writer, Vec3 vector) {
        writer.Write(prefix);
        writer.Write(" ");
        writer.Write(vector.X);
        writer.Write(" ");
        writer.Write(vector.Y);
        writer.Write(" ");
        writer.WriteLine(vector.Z);
    }

    private static void WriteFace(int i, TextWriter writer, Triangle tri) {
        // Write vertices
        WriteVector("v", writer, tri.Item1);
        WriteVector("v", writer, tri.Item2);
        WriteVector("v", writer, tri.Item3);

        // Write normals
        WriteVector("vn", writer, tri.Normal);

        // Write faces
        writer.Write("f ");
        writer.Write(i*3 + 1);  // Vertex
        writer.Write("//");
        writer.Write(i);        // Normal

        writer.Write(' ');
        writer.Write(i*3 + 2);  // Vertex
        writer.Write("//");
        writer.Write(i);        // Normal

        writer.Write(' ');
        writer.Write(i*3 + 3);  // Vertex
        writer.Write("//");
        writer.WriteLine(i);    // Normal
    }

    /// <summary>
    /// Write solid to object format
    /// </summary>
    /// <param name="solid">solid to encode</param>
    /// <returns>object text</returns>
    public string Serialize(IMesh solid) {
        var writer = new StringWriter();
        using (writer) {
            writer.WriteLine("# Wavefront Object");
            int i = 0;
            foreach (Triangle tri in solid) {
                WriteFace(i++, writer, tri);
            }
            writer.Flush();
        }
        return writer.ToString();
    }

    /// <summary>
    /// Read an ASCII Obj file
    /// </summary>
    /// <param name="reader">input text</param>
    /// <returns>solid</returns>
    public ListMesh Deserialize (TextReader reader) {
        List<Triangle> triangles = new List<Triangle>();
        List<Vec3> vectors = new List<Vec3>();
        List<int> faces = new List<int>();

        // Read data
        string line;
        while((line = reader.ReadLine()) != null) {
            if(line.StartsWith("v ")) {
                // Is vertex (v x y z)
                string[] parts = line.Split(null);
                vectors.Add(new Vec3(
                    parts.Length >= 2 ? double.Parse(parts[1]) : 0,
                    parts.Length >= 3 ? double.Parse(parts[2]) : 0,
                    parts.Length >= 4 ? double.Parse(parts[3]) : 0
                ));
            } else if (line.StartsWith("f ")) {
                // Is face (f v/u/n v/u/n v/u/n)
                string[] parts = line.Split(null);
                // TODO, handle more than 3 vertices
                faces.Add(int.Parse(Regex.Match(parts[1], @"\d+").Value));
                faces.Add(int.Parse(Regex.Match(parts[2], @"\d+").Value));
                faces.Add(int.Parse(Regex.Match(parts[3], @"\d+").Value));
            }
        }

        // Create triangles
        for(int i = 0; i < faces.Count; i+=3) {
            triangles.Add(new Triangle(
                vectors[faces[ i   ] - 1],
                vectors[faces[ i+1 ] - 1],
                vectors[faces[ i+2 ] - 1]
            ));
        }

        return new ListMesh(triangles);
    }
}

}