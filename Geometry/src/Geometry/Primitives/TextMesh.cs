using System;
using System.Linq;
using System.Collections.Generic;
using Qkmaxware.Geometry.IO;
using System.IO;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// 3-dimensional font
/// </summary>
public class Font3 : Dictionary<char, IMesh> {

    /// <summary>
    /// Width of a character
    /// </summary>
    public double CharWidth {get; private set;}
    /// <summary>
    /// height of a line
    /// </summary>
    public double LineHeight {get; private set;}
    /// <summary>
    /// Font size
    /// </summary>
    public double Size {get; private set;}

    private static Font3? defaultFont = null;

    /// <summary>
    /// Fetch the default font, loading from disk if required
    /// </summary>
    /// <returns>Default font</returns>
    public static Font3 Default() {
        if (Font3.defaultFont != null) {
            return Font3.defaultFont;
        }

        var assembly = typeof(Font3).Assembly;
        var defaultFontFolder = assembly.GetName().Name + ".Fonts.Bfont.";
        Dictionary<char, IMesh> defaultFontCharSet = new Dictionary<char, IMesh>();
        StlSerializer serializer = new StlSerializer();

        foreach (var resource in assembly.GetManifestResourceNames().Where(name => name.StartsWith(defaultFontFolder) && name.EndsWith(".stl"))) {
            var filename = Path.GetFileNameWithoutExtension(resource.Substring(defaultFontFolder.Length));
            
            using(var stream = assembly.GetManifestResourceStream(resource)) {
                if (stream == null)
                    continue;
                try {
                    using (var reader = new BinaryReader(stream)) {
                        var mesh = serializer.Deserialize(reader);
                        var @char = (char)(int.Parse(filename));
                        defaultFontCharSet[@char] = mesh;
                    }
                } catch { 
                    // Eat bad characters just in case. We don't want failure here
                }
            }
            
        }
        
        Font3.defaultFont = new Font3(1, 1, defaultFontCharSet);
        return Font3.defaultFont;
    }

    /// <summary>
    /// Create a new font
    /// </summary>
    /// <param name="charWidth">character width</param>
    /// <param name="lineHeight">character height</param>
    /// <param name="characterSet">character set</param>
    /// <returns>font</returns>
    public Font3 (double charWidth, double lineHeight, Dictionary<char, IMesh> characterSet) : base(characterSet) {
        this.CharWidth = charWidth;
        this.LineHeight = lineHeight;
        this.Size = 1;
    }

    /// <summary>
    /// Return null or mesh if the given character exists in the character set
    /// </summary>
    /// <param name="c">character to check</param>
    /// <returns>Mesh if character exists, null otherwise</returns>
    public IMesh? MeshForChar(char c) {
        if (this.ContainsKey(c)) {
            return this[c];
        } else {
            return null;
        }
    }

    /// <summary>
    /// Compute the dimensions of a string
    /// </summary>
    /// <param name="str">string to analyze</param>
    /// <returns>width and height of the resultant mesh</returns>
    public (double width, double height) StringMetrics(string str) {
        if (str == null)
            return (width: 0, height: 0);

        var lines = str.Split('\n');

        if (lines.Length == 0)
            return (width: 0, height: 0);

        var width = lines.Select(ln => ln.Length).Max();
        var height = lines.Length;

        return (width: width * this.CharWidth, height: height * this.LineHeight);
    }

    /// <summary>
    /// Create a new font my changing the font size of the existing font
    /// </summary>
    /// <param name="size">new size</param>
    /// <returns>new font derived from the old font</returns>
    public Font3 DeriveFont(double size) {
        var ratio = size / this.Size;
        var transformation = Transformation.Scale(ratio * Vec3.One);
        Dictionary<char, IMesh> newSet = new Dictionary<char, IMesh>();
        foreach (var map in this) {
            newSet.Add(map.Key, transformation * map.Value);
        }
        var font = new Font3(ratio * this.CharWidth, ratio * this.LineHeight, newSet);
        font.Size = size;
        return font;
    }

}

/// <summary>
/// Mesh representing a string of text
/// </summary>
public class TextMesh : ListMesh {

    /// <summary>
    /// Textual value of this mesh
    /// </summary>
    /// <value></value>
    public string Text {get; private set;}

    /// <summary>
    /// Create a new text mesh with the default font
    /// </summary>
    /// <param name="text">text</param>
    public TextMesh (string text) : this(Font3.Default(), text) {}

    /// <summary>
    /// Create a new text mesh with the given font
    /// </summary>
    /// <param name="font">font</param>
    /// <param name="text">text</param>
    public TextMesh(Font3 font, string text) {
        this.Text = text;
        RegenerateMesh(font);
    }

    /// <summary>
    /// Rebuild the mesh triangles
    /// </summary>
    /// <param name="font">font to rebuild with</param>
    private void RegenerateMesh(Font3 font) {
        this.Clear();

        double cursorLeft = 0;
        double cursorTop = 0;
        if (Text != null) {
            for (var i = 0; i < Text.Length; i++) {
                // handle whitespace / newlines
                char c = Text[i];
                if (c == '\n') {
                    cursorTop += font.LineHeight;
                    cursorLeft = 0;
                    continue;
                }
                else if (char.IsWhiteSpace(c)) {
                    cursorLeft += font.CharWidth;
                    continue;
                }

                // Position character
                var mesh = font.MeshForChar(c);
                if (mesh != null) {
                    var offset = new Vec3(cursorLeft, -cursorTop, 0);
                    var positionedMesh = Transformation.Offset(offset) * mesh;
                    this.AppendRange(positionedMesh);
                    cursorLeft += font.CharWidth; // Move cursor
                }
            }
        }
    }
}

}